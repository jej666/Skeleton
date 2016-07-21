﻿using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    [DebuggerDisplay("EntityName = {EntityType.Name}")]
    internal abstract class SqlBuilderBase<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IMetadata _metadata;
        private string _cacheIdName;

        internal SqlBuilderBase(IMetadataProvider metadataProvider)
        {
            metadataProvider.ThrowIfNull(() => metadataProvider);

            _metadata = metadataProvider.GetMetadata<TEntity>();
        }

        internal IDictionary<string, object> Parameters
        {
            get { return ContextBase.Parameters; }
        }

        internal abstract string SqlQuery { get; }
        protected internal abstract string SqlQueryTemplate { get; }
        protected internal abstract ContextBase ContextBase { get; }

        internal abstract void OnNextQuery();

        internal T OnNextQuery<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            finally { OnNextQuery(); }
        }

        protected internal Type EntityType
        {
            get { return _metadata.Type; }
        }

        protected internal string TableName
        {
            get { return EntityType.Name; }
        }

        protected internal string EntityIdName
        {
            get
            {
                if (!_cacheIdName.IsNullOrEmpty())
                    return _cacheIdName;

                var instance = _metadata.CreateInstance<TEntity>();

                if (instance == null)
                    return string.Empty;

                _cacheIdName = instance.IdAccessor.Name;

                return _cacheIdName;
            }
        }

        protected internal static IEnumerable<IMemberAccessor> GetTableColumns(TEntity entity)
        {
            return entity.TypeAccessor.GetDeclaredOnlyProperties()
                .Where(x => x.MemberType.IsPrimitiveExtended())
                .ToArray();
        }

        internal void Build(Node node)
        {
            Build((dynamic)node);
        }

        internal void Build(LikeNode node)
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

        internal void Build(OperationNode node)
        {
            Build((dynamic)node.Left, (dynamic)node.Right, node.Operator);
        }

        internal void Build(MemberNode memberNode)
        {
            QueryFieldCondition(
                memberNode,
                SqlFormatter.Operations[ExpressionType.Equal],
                true);
        }

        internal void Build(SingleOperationNode node)
        {
            if (node.Operator == ExpressionType.Not)
                Not();

            Build(node.Child);
        }

        private void Build(MemberNode memberNode, ValueNode valueNode, ExpressionType op)
        {
            if (valueNode.Value == null)
            {
                ResolveNullValue(memberNode, op);
            }
            else
            {
                QueryFieldCondition(
                    memberNode,
                    SqlFormatter.Operations[op],
                    valueNode.Value);
            }
        }

        private void Build(ValueNode valueNode, MemberNode memberNode, ExpressionType op)
        {
            Build(memberNode, valueNode, op);
        }

        private void Build(MemberNode leftNode, MemberNode rightNode, ExpressionType op)
        {
            var newCondition = SqlFormatter.FieldComparison(
                leftNode,
                SqlFormatter.Operations[op],
                rightNode);

            ContextBase.Conditions.Add(newCondition);
        }

        private void Build(SingleOperationNode leftMember, Node rightMember, ExpressionType op)
        {
            if (leftMember.Operator == ExpressionType.Not)
                Build(leftMember as Node, rightMember, op);
            else
                Build((dynamic)leftMember.Child, (dynamic)rightMember, op);
        }

        private void Build(Node leftMember, SingleOperationNode rightMember, ExpressionType op)
        {
            Build(rightMember, leftMember, op);
        }

        private void Build(Node leftNode, Node rightNode, ExpressionType op)
        {
            BeginExpression();
            Build((dynamic)leftNode);
            ResolveOperation(op);
            Build((dynamic)rightNode);
            EndExpression();
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
            var expressionTree = ExpressionResolver.Resolve((dynamic)expression.Body);
            And();
            Build(expressionTree);
        }

        protected internal void QueryByPrimaryKey(Expression<Func<TEntity, bool>> whereExpression)
        {
            var expressionTree = ExpressionResolver.Resolve((dynamic)whereExpression.Body, EntityIdName);
            Build(expressionTree);
        }

        protected internal void QueryFieldCondition(MemberNode node, string op, object fieldValue)
        {
            var paramId = ContextBase.NextParamId();
            var newCondition = SqlFormatter.FieldCondition(node, op, paramId);

            ContextBase.Conditions.Add(newCondition);
            ContextBase.AddParameter(paramId, fieldValue);
        }

        protected internal void And()
        {
            if (ContextBase.Conditions.IsNotNullOrEmpty())
                ContextBase.Conditions.Add(SqlFormatter.AndExpression);
        }

        protected internal void BeginExpression()
        {
            ContextBase.Conditions.Add(SqlFormatter.BeginExpression);
        }

        protected internal void EndExpression()
        {
            ContextBase.Conditions.Add(SqlFormatter.EndExpression);
        }

        protected internal void Not()
        {
            ContextBase.Conditions.Add(SqlFormatter.NotExpression);
        }

        protected internal void Or()
        {
            if (ContextBase.Conditions.IsNotNullOrEmpty())
                ContextBase.Conditions.Add(SqlFormatter.OrExpression);
        }

        protected internal void ResolveNullValue(MemberNode node, ExpressionType op)
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

        protected internal void ResolveOperation(ExpressionType op)
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
    }
}