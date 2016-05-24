//https://github.com/base33/lambda-sql-builder

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Skeleton.Common.Extensions;
using Skeleton.Core.Repository;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    public class SqlBuilderImpl : ISqlQuery, ISqlExecute
    {
        private readonly SqlQueryBuilder _builder;

        internal SqlBuilderImpl(Type type)
        {
            _builder = new SqlQueryBuilder(type.Name);
        }

        public string DeleteQuery
        {
            get { return _builder.DeleteQuery; }
        }

        public string InsertQuery
        {
            get { return _builder.InsertQuery; }
        }

        public string UpdateQuery
        {
            get { return _builder.UpdateQuery; }
        }

        public IDictionary<string, object> Parameters
        {
            get { return _builder.Parameters; }
        }

        public string Query
        {
            get { return _builder.Query; }
        }

        public string PagedQuery(int pageSize, int pageNumber)
        {
            return _builder.PagedQuery(pageSize, pageNumber);
        }

        internal void And()
        {
            _builder.And();
        }

        internal void Or()
        {
            _builder.Or();
        }

        internal void Insert(string columnName, object value)
        {
            _builder.Insert(columnName, value);
        }

        internal void Update(string columnName, object value)
        {
            _builder.Update(columnName, value);
        }

        internal void GroupBy<T>(Expression<Func<T, object>> expression)
        {
            GroupBy<T>(expression.Body.GetMemberExpression());
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
            var expressionTree = ExpressionResolver.ResolveQuery((dynamic) whereExpression.Body, idColumnName);
            _builder.BuildSql(expressionTree);
        }

        internal void ResolveQuery<T>(Expression<Func<T, bool>> expression)
        {
            var expressionTree = ExpressionResolver.ResolveQuery((dynamic) expression.Body);
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
                    var newExpression = expression as NewExpression;
                    if (newExpression != null)
                        foreach (var expr in newExpression.Arguments)
                        {
                            var memberExp = (MemberExpression) expr;
                            Select<T>(memberExp);
                        }
                    break;

                default:
                    throw new ArgumentException("Invalid expression");
            }
        }

        private void Select<T>(MemberExpression expression)
        {
            if (expression.Type.IsClass && expression.Type != typeof(string))
                _builder.Select(TableInfo.GetTableName(expression.Type));
            else
                _builder.Select(TableInfo.GetTableName<T>(), TableInfo.GetColumnName(expression));
        }

        private void SelectWithFunction<T>(Expression expression, SelectFunction selectFunction)
        {
            var fieldName = TableInfo.GetColumnName(expression.GetMemberExpression());
            _builder.Select(TableInfo.GetTableName<T>(), fieldName, selectFunction);
        }

        internal void Join<T1, T2>(Expression<Func<T1, T2, bool>> expression, JoinType joinType)
        {
            var joinExpression = expression.Body.GetBinaryExpression();
            var leftExpression = joinExpression.Left.GetMemberExpression();
            var rightExpression = joinExpression.Right.GetMemberExpression();

            Join<T1, T2>(leftExpression, rightExpression, joinType);
        }

        private void Join<T1, T2>(MemberExpression leftExpression, MemberExpression rightExpression, JoinType joinType)
        {
            _builder.Join(
                TableInfo.GetTableName<T1>(),
                TableInfo.GetTableName<T2>(),
                TableInfo.GetColumnName(leftExpression),
                TableInfo.GetColumnName(rightExpression),
                joinType);
        }
    }
}