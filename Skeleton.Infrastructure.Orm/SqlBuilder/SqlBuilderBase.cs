using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Reflection;
using Skeleton.Core;
using Skeleton.Infrastructure.Orm.ExpressionTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Orm.SqlBuilder
{
    [DebuggerDisplay("EntityName = {TableName}")]
    internal abstract class SqlBuilderBase<TEntity>
        where TEntity : class, IEntity<TEntity>, new()
    {
        private readonly IMetadata _metadata;
        private readonly IInstanceAccessor _entityInstanceAccessor;
        private readonly Type _entityType;
        private string _cacheIdName;

        protected internal SqlBuilderBase(IMetadataProvider metadataProvider)
        {
            metadataProvider.ThrowIfNull(nameof(metadataProvider));

            _metadata = metadataProvider.GetMetadata<TEntity>();
            _entityType = _metadata.Type;
            _entityInstanceAccessor = _metadata.GetConstructor();
        }

        internal abstract string SqlQuery { get; }

        protected internal abstract string SqlQueryTemplate { get; }

        internal ISqlCommand SqlCommand => new SqlCommand(SqlQuery, ContextBase.Parameters);

        protected internal abstract ContextBase ContextBase { get; }

        internal string TableName => _entityType.Name;

        internal string EntityIdName
        {
            get
            {
                if (_cacheIdName.IsNotNullOrEmpty())
                    return _cacheIdName;

                var instance = _entityInstanceAccessor.InstanceCreator(null) as TEntity;

                if (instance == null)
                    return string.Empty;

                _cacheIdName = instance.IdAccessor.Name;

                return _cacheIdName;
            }
        }

        internal abstract void OnNextQuery();

        internal T OnNextQuery<T>(Func<T> func)
        {
            try
            {
                func.ThrowIfNull(nameof(func));

                return func();
            }
            finally
            {
                OnNextQuery();
            }
        }

        internal static IEnumerable<IMemberAccessor> GetTableColumns(TEntity entity)
        {
            return entity.Metadata
                .GetDeclaredOnlyProperties()
                .Where(x => x.MemberType.IsPrimitiveExtended())
                .ToArray();
        }

        internal void WhereIsIn(Expression<Func<TEntity, object>> expression, IEnumerable<object> values)
        {
            var fieldName = TableInfo.GetColumnName(expression);
            var memberNode = new MemberNode { TableName = TableName, FieldName = fieldName };

            And();
            WhereIsIn(memberNode, values);
        }

        internal void WhereNotIn(Expression<Func<TEntity, object>> expression, IEnumerable<object> values)
        {
            var fieldName = TableInfo.GetColumnName(expression);
            var memberNode = new MemberNode { TableName = TableName, FieldName = fieldName };

            Not();
            WhereIsIn(memberNode, values);
        }

        private void WhereIsIn(MemberNode node, IEnumerable<object> values)
        {
            var paramIds = values.Select(x =>
            {
                var paramId = ContextBase.NextParamId();
                ContextBase.AddParameter(paramId, x);

                return SqlFormatter.Parameter(paramId);
            });

            var newCondition = SqlFormatter.WhereIsIn(node, paramIds);
            ContextBase.Conditions.Add(newCondition);
        }

        internal void ResolveQuery(Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(nameof(expression));

            var expressionTree = ExpressionResolver.Resolve((dynamic)expression.Body);
            And();
            Build(expressionTree);
        }

        internal void QueryByPrimaryKey(Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(nameof(expression));

            var expressionTree = ExpressionResolver.Resolve((dynamic)expression.Body, EntityIdName);
            Build(expressionTree);
        }

        private void QueryFieldCondition(MemberNode node, string op, object fieldValue)
        {
            var paramId = ContextBase.NextParamId();
            var newCondition = SqlFormatter.FieldCondition(node, op, paramId);

            ContextBase.Conditions.Add(newCondition);
            ContextBase.AddParameter(paramId, fieldValue);
        }

        private void And()
        {
            if (ContextBase.Conditions.IsNotNullOrEmpty())
                ContextBase.Conditions.Add(SqlFormatter.AndExpression);
        }

        private void Not()
        {
            ContextBase.Conditions.Add(SqlFormatter.NotExpression);
        }

        private void Or()
        {
            if (ContextBase.Conditions.IsNotNullOrEmpty())
                ContextBase.Conditions.Add(SqlFormatter.OrExpression);
        }

        private void ResolveNullValue(MemberNode node, ExpressionType op)
        {
            switch (op)
            {
                case ExpressionType.Equal:
                    ContextBase.Conditions.Add(SqlFormatter.FieldIsNull(node));
                    break;

                case ExpressionType.NotEqual:
                    ContextBase.Conditions.Add(SqlFormatter.FieldIsNotNull(node));
                    break;
            }
        }

        private void ResolveOperation(ExpressionType op)
        {
            switch (op)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    And();
                    break;

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    Or();
                    break;

                default:
                    throw new ArgumentException(
                        "Unrecognized binary expression operation '{0}'"
                            .FormatWith(op.ToString()));
            }
        }

        private void Build(OperationNode node)
        {
            Build((dynamic)node.Left, (dynamic)node.Right, node.Operator);
        }

        private void Build(MemberNode memberNode)
        {
            QueryFieldCondition(
                memberNode,
                SqlFormatter.Operations[ExpressionType.Equal],
                true);
        }

        private void Build(SingleOperationNode node)
        {
            if (node.Operator == ExpressionType.Not)
                Not();

            Build(node.Child);
        }

        private void Build(MemberNode memberNode, ValueNode valueNode, ExpressionType op)
        {
            if (valueNode.Value == null)
                ResolveNullValue(memberNode, op);
            else
                QueryFieldCondition(
                    memberNode,
                    SqlFormatter.Operations[op],
                    valueNode.Value);
        }

        private void Build(ValueNode valueNode, MemberNode memberNode, ExpressionType op)
        {
            Build(memberNode, valueNode, op);
        }

        private void Build(SingleOperationNode leftMember, NodeBase rightMember, ExpressionType op)
        {
            if (leftMember.Operator == ExpressionType.Not)
                Build(leftMember as NodeBase, rightMember, op);
            else
                Build((dynamic)leftMember.Child, (dynamic)rightMember, op);
        }

        private void Build(NodeBase leftMember, SingleOperationNode rightMember, ExpressionType op)
        {
            Build(rightMember, leftMember, op);
        }

        private void Build(NodeBase leftNode, NodeBase rightNode, ExpressionType op)
        {
            ContextBase.Conditions.Add(SqlFormatter.BeginExpression);
            Build((dynamic)leftNode);
            ResolveOperation(op);
            Build((dynamic)rightNode);
            ContextBase.Conditions.Add(SqlFormatter.EndExpression);
        }

        private void Build(MemberNode leftNode, MemberNode rightNode, ExpressionType op)
        {
            var newCondition = SqlFormatter.FieldComparison(
                leftNode,
                SqlFormatter.Operations[op],
                rightNode);

            ContextBase.Conditions.Add(newCondition);
        }

        private void Build(NodeBase node)
        {
            Build((dynamic)node);
        }

        private void Build(LikeNode node)
        {
            if (node.Method == LikeMethod.Equals)
            {
                QueryFieldCondition(node.MemberNode,
                    SqlFormatter.Operations[ExpressionType.Equal],
                    node.Value);
            }
            else
            {
                var value = SqlFormatter.LikeCondition(node.Value, node.Method);
                var paramId = ContextBase.NextParamId();
                var newCondition = SqlFormatter.FieldLike(node.MemberNode, paramId);

                ContextBase.Conditions.Add(newCondition);
                ContextBase.AddParameter(paramId, value);
            }
        }
    }
}