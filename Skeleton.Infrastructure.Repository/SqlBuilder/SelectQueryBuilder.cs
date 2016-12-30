using System;
using System.Linq.Expressions;
using Skeleton.Abstraction;
using Skeleton.Common;
using Skeleton.Infrastructure.Repository.ExpressionTree;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    public class SelectQueryBuilder<TEntity> :
            SqlBuilderBase<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        public SelectQueryBuilder(IMetadataProvider metadataProvider)
            : base(metadataProvider)
        {
        }

        protected override ContextBase ContextBase => Context;

        protected QueryContext Context { get; private set; } = new QueryContext();

        public override string SqlQuery => SqlQueryTemplate
            .FormatWith(
                Context.Top,
                SqlFormatter.SelectedColumns(Context.Selection, TableName),
                SqlFormatter.Source(Context.Source, TableName),
                SqlFormatter.Conditions(Context.Conditions),
                SqlFormatter.GroupBy(Context.GroupBy),
                SqlFormatter.Having(Context.Having),
                SqlFormatter.OrderBy(Context.OrderBy));

        protected override string SqlQueryTemplate => "SELECT {0} {1} FROM {2} {3} {4} {5} {6}";

        public override void OnNextQuery()
        {
            Context = new QueryContext();
        }

        public void OrderBy(LambdaExpression expression)
        {
            expression.ThrowIfNull(() => expression);

            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            var memberNode = new MemberNode {TableName = TableName, FieldName = fieldName};

            Context.OrderBy.Add(SqlFormatter.Field(memberNode));
        }

        public void OrderByDescending(LambdaExpression expression)
        {
            expression.ThrowIfNull(() => expression);

            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            var memberNode = new MemberNode {TableName = TableName, FieldName = fieldName};

            Context.OrderBy.Add(SqlFormatter.OrderByDescending(memberNode));
        }

        public void Top(int take)
        {
            Context.Top = SqlFormatter.Top(take);
        }

        public void Count()
        {
            Context.Selection.Add(SqlFormatter.CountAny);
        }

        public void Aggregate(LambdaExpression expression, SelectFunction selectFunction)
        {
            expression.ThrowIfNull(() => expression);

            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            var memberNode = new MemberNode {TableName = TableName, FieldName = fieldName};

            var selectionString =
                SqlFormatter.SelectAggregate(memberNode, selectFunction.ToString());

            Context.Selection.Add(selectionString);
        }

        public void GroupBy(LambdaExpression expression)
        {
            expression.ThrowIfNull(() => expression);

            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            var memberNode = new MemberNode {TableName = TableName, FieldName = fieldName};

            Context.GroupBy.Add(SqlFormatter.Field(memberNode));
        }

        public void Select(LambdaExpression expression)
        {
            expression.ThrowIfNull(() => expression);

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
                            var memberExp = (MemberExpression) e;
                            Select(memberExp);
                        }
                    break;

                default:
                    throw new ArgumentException("Invalid expression");
            }
        }

        private void Select(MemberExpression expression)
        {
            if (expression.Type.IsClass && (expression.Type != typeof(string)))
                Select(TableInfo.GetTableName(expression.Type));
            else
            {
                var node = new MemberNode
                {
                    TableName = TableName,
                    FieldName = TableInfo.GetColumnName(expression)
                };

                Context.Selection.Add(SqlFormatter.Field(node));
            }
        }

        public void Join<T1, T2>(
            Expression<Func<T1, T2, bool>> expression, JoinType joinType)
        {
            expression.ThrowIfNull(() => expression);

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

            Context.TableNames.Add(joinTableName);
            Context.Source.Add(joinString);
        }

        private void Select(string tableName)
        {
            Context.Selection.Add(SqlFormatter.SelectAll(tableName));
        }

        protected void OrderByPrimaryKey()
        {
            var memberNode = new MemberNode {TableName = TableName, FieldName = EntityIdName};
            Context.OrderBy.Add(SqlFormatter.Field(memberNode));
        }
    }
}