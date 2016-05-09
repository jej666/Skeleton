//https://github.com/base33/lambda-sql-builder
namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    using Common.Extensions;
    using Core.Repository;
    using ExpressionTree;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    internal sealed class LambdaExpressionResolver
    {
        private readonly InternalQueryBuilder _builder;

        internal LambdaExpressionResolver(InternalQueryBuilder builder)
        {
            _builder = builder;
        }

        internal void GroupBy<T>(Expression<Func<T, object>> expression)
        {
            GroupBy<T>(expression.Body.GetMemberExpression());
        }

        internal void Join<T1, T2>(Expression<Func<T1, T2, bool>> expression)
        {
            var joinExpression = expression.Body.GetBinaryExpression();
            var leftExpression = joinExpression.Left.GetMemberExpression();
            var rightExpression = joinExpression.Right.GetMemberExpression();

            Join<T1, T2>(leftExpression, rightExpression);
        }

        internal void Join<T1, T2, TKey>(Expression<Func<T1, TKey>> leftExpression, Expression<Func<T1, TKey>> rightExpression)
        {
            Join<T1, T2>(leftExpression.Body.GetMemberExpression(), rightExpression.Body.GetMemberExpression());
        }

        internal void Join<T1, T2>(MemberExpression leftExpression, MemberExpression rightExpression)
        {
            _builder.Join(
                TableInfo.GetTableName<T1>(),
                TableInfo.GetTableName<T2>(),
                TableInfo.GetColumnName(leftExpression),
                TableInfo.GetColumnName(rightExpression));
        }

        internal void OrderBy<T>(Expression<Func<T, object>> expression)
        {
            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            _builder.OrderBy(TableInfo.GetTableName<T>(), fieldName);
        }

        internal void OrderByDescending<T>(Expression<Func<T, object>> expression)
        {
            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            _builder.OrderByDescending(TableInfo.GetTableName<T>(), fieldName);
        }

        internal void QueryByIsIn<T>(Expression<Func<T, object>> expression, ISqlQuery sqlQuery)
        {
            var fieldName = TableInfo.GetColumnName(expression);
            _builder.QueryByIsIn(TableInfo.GetTableName<T>(), fieldName, sqlQuery);
        }

        internal void QueryByIsIn<T>(Expression<Func<T, object>> expression, IEnumerable<object> values)
        {
            var fieldName = TableInfo.GetColumnName(expression);
            _builder.QueryByIsIn(TableInfo.GetTableName<T>(), fieldName, values);
        }

        internal void QueryByNotIn<T>(Expression<Func<T, object>> expression, ISqlQuery sqlQuery)
        {
            var fieldName = TableInfo.GetColumnName(expression);
            _builder.Not();
            _builder.QueryByIsIn(TableInfo.GetTableName<T>(), fieldName, sqlQuery);
        }

        internal void QueryByNotIn<T>(Expression<Func<T, object>> expression, IEnumerable<object> values)
        {
            var fieldName = TableInfo.GetColumnName(expression);
            _builder.Not();
            _builder.QueryByIsIn(TableInfo.GetTableName<T>(), fieldName, values);
        }

        internal void QueryByPrimaryKey<T>(
            string idColumnName,
            Expression<Func<T, bool>> whereExpression)
        {
            var expressionTree = InternalQueryResolver.ResolveQuery((dynamic)whereExpression.Body, idColumnName);
            _builder.BuildSql(expressionTree);
        }

        internal void ResolveQuery<T>(Expression<Func<T, bool>> expression)
        {
            var expressionTree = InternalQueryResolver.ResolveQuery((dynamic)expression.Body);
            _builder.BuildSql(expressionTree);
        }

        internal void Select<T>(Expression<Func<T, object>> expression)
        {
            Select<T>(expression.Body);
        }

        internal void SelectTop(int take)
        {
            _builder.SelectTop(take);
        }

        internal void SelectWithFunction<T>(Expression<Func<T, object>> expression, SelectFunction selectFunction)
        {
            SelectWithFunction<T>(expression.Body, selectFunction);
        }

        private void GroupBy<T>(MemberExpression expression)
        {
            var fieldName = TableInfo.GetColumnName(expression.GetMemberExpression());
            _builder.GroupBy(TableInfo.GetTableName<T>(), fieldName);
        }

        private void ResolveSingleOperation(ExpressionType op)
        {
            switch (op)
            {
                case ExpressionType.Not:
                    _builder.Not();
                    break;
            }
        }

        private void Select<T>(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Parameter:
                    _builder.Select(TableInfo.GetTableName(expression.Type));
                    break;

                case ExpressionType.Convert:
                case ExpressionType.MemberAccess:
                    Select<T>(expression.GetMemberExpression());
                    break;

                case ExpressionType.New:
                    foreach (MemberExpression memberExp in (expression as NewExpression).Arguments)
                        Select<T>(memberExp);
                    break;

                default:
                    throw new ArgumentException("Invalid expression");
            }
        }

        private void Select<T>(MemberExpression expression)
        {
            if (expression.Type.IsClass && expression.Type != typeof(String))
                _builder.Select(TableInfo.GetTableName(expression.Type));
            else
                _builder.Select(TableInfo.GetTableName<T>(), TableInfo.GetColumnName(expression));
        }

        private void SelectWithFunction<T>(Expression expression, SelectFunction selectFunction)
        {
            var fieldName = TableInfo.GetColumnName(expression.GetMemberExpression());
            _builder.Select(TableInfo.GetTableName<T>(), fieldName, selectFunction);
        }
    }
}