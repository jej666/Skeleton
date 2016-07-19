using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    [DebuggerDisplay("EntityName = {EntityType.Name}")]
    internal sealed class QueryBuilder<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private QueryContext _context;
        private readonly IMetadata _metadata;
        private string _cacheIdName;

        internal QueryBuilder(IMetadataProvider metadataProvider)
        {
            metadataProvider.ThrowIfNull(() => metadataProvider);

            _metadata = metadataProvider.GetMetadata<TEntity>();
            Initialize();
        }

        internal Type EntityType
        {
            get { return _metadata.Type; }
        }

        internal string TableName
        {
            get { return EntityType.Name; }
        }

        internal string EntityIdName
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

        private static IEnumerable<IMemberAccessor> GetTableColumns(TEntity entity)
        {
            return entity.TypeAccessor.GetDeclaredOnlyProperties()
                .Where(x => x.MemberType.IsPrimitiveExtended())
                .ToArray();
        }

        internal void SetInsertColumns(TEntity entity)
        {
            foreach (var column in GetTableColumns(entity))
            {
                if (entity.IdAccessor.Name.IsNullOrEmpty() ||
                    entity.IdAccessor.Name == column.Name)
                    continue;

                var value = column.GetValue(entity);
                var formattedValue = SqlFormatter.GetFormattedValue(value);

                _context.AddInsert(column.Name, formattedValue);
            }
        }

        internal void SetUpdateColumns(TEntity entity)
        {
            foreach (var column in GetTableColumns(entity))
            {
                if (entity.IdAccessor.Name.IsNullOrEmpty() ||
                    entity.IdAccessor.Name == column.Name)
                    continue;

                var value = column.GetValue(entity);
                var columnValue = SqlFormatter.ColumnValue(
                    TableName, column.Name, value);

                _context.AddUpdate(columnValue);
            }
        }

        internal void Initialize()
        {
            _context = new QueryContext(TableName, EntityIdName);
        }

        internal T Initialize<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            finally
            {
                Initialize();
            }
        }

        internal string DeleteQuery
        {
            get
            {
                return SqlFormatter.DeleteQuery
                    .FormatWith(
                        TableName,
                        SqlFormatter.Conditions(_context.Conditions));
            }
        }

        internal string InsertQuery
        {
            get
            {
                return SqlFormatter.InsertQuery
                    .FormatWith(
                        TableName,
                        SqlFormatter.Fields(_context.Columns),
                        SqlFormatter.Fields(_context.InsertValues));
            }
        }

        internal string SelectQuery
        {
            get
            {
                return SqlFormatter.SelectQuery
                    .FormatWith(
                        _context.Top,
                        SqlFormatter.Selection(_context.Selection, TableName),
                        SqlFormatter.Source(_context.Source, TableName),
                        SqlFormatter.Conditions(_context.Conditions),
                        SqlFormatter.Grouping(_context.Grouping),
                        SqlFormatter.Having(_context.Having),
                        SqlFormatter.Ordering(_context.Order));
            }
        }

        internal string UpdateQuery
        {
            get
            {
                return SqlFormatter.UpdateQuery
                    .FormatWith(
                        TableName,
                        SqlFormatter.Fields(_context.UpdateColumnValues),
                        SqlFormatter.Conditions(_context.Conditions));
            }
        }

        internal string PagedQuery(int pageSize, int pageNumber)
        {
            if (_context.Order.IsNullOrEmpty())
                _context.AddOrderBy(
                    TableName,
                    EntityIdName);

            return SqlFormatter.PagedQuery
                .FormatWith(
                    SqlFormatter.Selection(_context.Selection, TableName),
                    SqlFormatter.Source(_context.Source, TableName),
                    SqlFormatter.Conditions(_context.Conditions),
                    SqlFormatter.Ordering(_context.Order),
                    pageSize * (pageNumber - 1),
                    pageSize);
        }

        internal IDictionary<string, object> Parameters
        {
            get { return _context.Parameters; }
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
                var value = node.Value;
                switch (node.Method)
                {
                    case LikeMethod.StartsWith:
                        value = node.Value + "%";
                        break;

                    case LikeMethod.EndsWith:
                        value = "%" + node.Value;
                        break;

                    case LikeMethod.Contains:
                        value = "%" + node.Value + "%";
                        break;
                }

                var paramId = _context.NextParamId();
                var newCondition = SqlFormatter.FieldLike(node.MemberNode, paramId);

                _context.AddCondition(newCondition);
                _context.AddParameter(paramId, value);
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
                _context.Not();

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

            _context.AddCondition(newCondition);
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
            _context.BeginExpression();
            Build((dynamic)leftNode);
            ResolveOperation(op);
            Build((dynamic)rightNode);
            _context.EndExpression();
        }

        internal void OrderBy(Expression<Func<TEntity, object>> expression)
        {
            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            _context.AddOrderBy(TableName, fieldName);
        }

        internal void OrderByDescending(Expression<Func<TEntity, object>> expression)
        {
            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            _context.AddOrderByDescending(TableName, fieldName);
        }

        internal void QueryByIsIn(Expression<Func<TEntity, object>> expression, IEnumerable<object> values)
        {
            var fieldName = TableInfo.GetColumnName(expression);
            var memberNode = new MemberNode { TableName = TableName, FieldName = fieldName };

            _context.And();
            QueryByIsIn(memberNode, values);
        }

        internal void QueryByNotIn(Expression<Func<TEntity, object>> expression, IEnumerable<object> values)
        {
            var fieldName = TableInfo.GetColumnName(expression);
            var memberNode = new MemberNode { TableName = TableName, FieldName = fieldName };

            _context.Not();
            QueryByIsIn(memberNode, values);
        }

        internal void QueryByPrimaryKey(Expression<Func<TEntity, bool>> whereExpression)
        {
            var expressionTree = ExpressionResolver.Resolve((dynamic)whereExpression.Body, EntityIdName);
            _context.And();
            Build(expressionTree);
        }

        internal void ResolveQuery(Expression<Func<TEntity, bool>> expression)
        {
            var expressionTree = ExpressionResolver.Resolve((dynamic)expression.Body);
            _context.And();
            Build(expressionTree);
        }

        internal void SelectTop(int take)
        {
            _context.AddSelectTop(take);
        }

        internal void SelectCount()
        {
            _context.AddSelectCount();
        }

        internal void SelectWithFunction<TResult>(
            Expression<Func<TEntity, TResult>> expression,
            SelectFunction selectFunction)
        {
            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            var memberNode = new MemberNode { TableName = TableName, FieldName = fieldName };
            var selectionString = SqlFormatter.SelectAggregate(memberNode, selectFunction.ToString());

            _context.AddSelect(selectionString);
        }

        internal void GroupBy(Expression expression)
        {
            var fieldName = TableInfo.GetColumnName(expression.GetMemberExpression());
            _context.AddGroupBy(TableName, fieldName);
        }

        internal void Select(Expression<Func<TEntity, object>> expression)
        {
            var expr = expression.Body;

            switch (expr.NodeType)
            {
                case ExpressionType.Parameter:
                    Select(TableInfo.GetTableName(expr.Type));
                    break;

                case ExpressionType.Convert:
                case ExpressionType.MemberAccess:
                    Select(expr.GetMemberExpression());
                    break;

                case ExpressionType.New:
                    var newExpression = expr as NewExpression;
                    if (newExpression != null)
                        foreach (var e in newExpression.Arguments)
                        {
                            var memberExp = (MemberExpression)e;
                            Select(memberExp);
                        }
                    break;

                default:
                    throw new ArgumentException("Invalid expression");
            }
        }

        private void Select(MemberExpression expression)
        {
            if (expression.Type.IsClass && expression.Type != typeof(string))
                Select(TableInfo.GetTableName(expression.Type));
            else
            {
                var node = new MemberNode
                {
                    TableName = TableName,
                    FieldName = TableInfo.GetColumnName(expression)
                };

                _context.AddSelect(SqlFormatter.Field(node));
            }
        }

        internal void Join<T1, T2>(Expression<Func<T1, T2, bool>> expression, JoinType joinType)
        {
            var joinExpression = expression.Body.GetBinaryExpression();
            var leftExpression = joinExpression.Left.GetMemberExpression();
            var rightExpression = joinExpression.Right.GetMemberExpression();
            var joinTableName = TableInfo.GetTableName<T2>();
            var joinString = SqlFormatter.Join(
                TableInfo.GetTableName<T1>(),
                joinTableName,
                TableInfo.GetColumnName(leftExpression),
                TableInfo.GetColumnName(rightExpression),
                joinType);

            _context.AddTableName(joinTableName);
            _context.AddJoin(joinString);
        }

        private void QueryFieldCondition(MemberNode node, string op, object fieldValue)
        {
            var paramId = _context.NextParamId();
            var newCondition = SqlFormatter.FieldCondition(node, op, paramId);

            _context.AddCondition(newCondition);
            _context.AddParameter(paramId, fieldValue);
        }

        internal void QueryByIsIn(MemberNode node, IEnumerable<object> values)
        {
            var paramIds = values.Select(x =>
            {
                var paramId = _context.NextParamId();
                _context.AddParameter(paramId, x);
                return SqlFormatter.Parameter(paramId);
            });

            var newCondition = SqlFormatter.QueryIsIn(node, paramIds);
            _context.AddCondition(newCondition);
        }

        internal void Select(string tableName)
        {
            var selectionString = "{0}.*".FormatWith(SqlFormatter.Table(tableName));
            _context.AddSelect(selectionString);
        }

        private void ResolveNullValue(MemberNode node, ExpressionType op)
        {
            switch (op)
            {
                case ExpressionType.Equal:
                    _context.AddCondition(SqlFormatter.FieldIsNull(node));
                    break;

                case ExpressionType.NotEqual:
                    _context.AddCondition(SqlFormatter.FieldIsNotNull(node));
                    break;
            }
        }

        private void ResolveOperation(ExpressionType op)
        {
            switch (op)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    _context.And();
                    break;

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    _context.Or();
                    break;

                default:
                    throw new ArgumentException(
                        "Unrecognized binary expression operation '{0}'"
                            .FormatWith(op.ToString()));
            }
        }
    }
}