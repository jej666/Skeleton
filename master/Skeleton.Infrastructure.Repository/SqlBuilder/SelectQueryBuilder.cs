﻿using System;
using System.Linq.Expressions;
using Skeleton.Abstraction;
using Skeleton.Common;
using Skeleton.Infrastructure.Repository.ExpressionTree;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal class SelectQueryBuilder<TEntity, TIdentity> :
            SqlBuilderBase<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        internal SelectQueryBuilder(IMetadataProvider metadataProvider)
            : base(metadataProvider)
        {
        }

        protected internal override ContextBase ContextBase => Context;

        protected internal QueryContext Context { get; private set; } = new QueryContext();

        internal override string SqlQuery => SqlQueryTemplate
            .FormatWith(
                Context.Top,
                SqlFormatter.SelectedColumns(Context.Selection, TableName),
                SqlFormatter.Source(Context.Source, TableName),
                SqlFormatter.Conditions(Context.Conditions),
                SqlFormatter.GroupBy(Context.GroupBy),
                SqlFormatter.Having(Context.Having),
                SqlFormatter.OrderBy(Context.OrderBy));

        internal static string SqlPagedQueryTemplate => "SELECT {0} FROM {1} {2} {3} OFFSET {4} ROWS FETCH NEXT {5} ROWS ONLY";

        protected internal override string SqlQueryTemplate => "SELECT {0} {1} FROM {2} {3} {4} {5} {6}";

        internal override void OnNextQuery()
        {
            Context = new QueryContext();
        }

        internal string SqlPagedQuery(int pageSize, int pageNumber)
        {
            if (Context.OrderBy.IsNullOrEmpty())
                OrderByPrimaryKey();

            return SqlPagedQueryTemplate
                .FormatWith(
                    SqlFormatter.SelectedColumns(Context.Selection, TableName),
                    SqlFormatter.Source(Context.Source, TableName),
                    SqlFormatter.Conditions(Context.Conditions),
                    SqlFormatter.OrderBy(Context.OrderBy),
                    pageSize*(pageNumber - 1),
                    pageSize);
        }

        internal void OrderBy(Expression<Func<TEntity, object>> expression)
        {
            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            var memberNode = new MemberNode {TableName = TableName, FieldName = fieldName};

            Context.OrderBy.Add(SqlFormatter.Field(memberNode));
        }

        internal void OrderByDescending(Expression<Func<TEntity, object>> expression)
        {
            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            var memberNode = new MemberNode {TableName = TableName, FieldName = fieldName};

            Context.OrderBy.Add(SqlFormatter.OrderByDescending(memberNode));
        }

        internal void Top(int take)
        {
            Context.Top = SqlFormatter.Top(take);
        }

        internal void Count()
        {
            Context.Selection.Add(SqlFormatter.CountAny);
        }

        internal void Aggregate(
            Expression<Func<TEntity, object>> expression,
            SelectFunction selectFunction)
        {
            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            var memberNode = new MemberNode {TableName = TableName, FieldName = fieldName};
            var selectionString = SqlFormatter.SelectAggregate(memberNode, selectFunction.ToString());

            Context.Selection.Add(selectionString);
        }

        internal void GroupBy(Expression<Func<TEntity, object>> expression)
        {
            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            var memberNode = new MemberNode {TableName = TableName, FieldName = fieldName};

            Context.GroupBy.Add(SqlFormatter.Field(memberNode));
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

        internal void Join<T1, T2>(
            Expression<Func<T1, T2, bool>> expression, JoinType joinType)
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

            Context.TableNames.Add(joinTableName);
            Context.Source.Add(joinString);
        }

        private void Select(string tableName)
        {
            Context.Selection.Add(SqlFormatter.SelectAll(tableName));
        }

        private void OrderByPrimaryKey()
        {
            var memberNode = new MemberNode {TableName = TableName, FieldName = EntityIdName};
            Context.OrderBy.Add(SqlFormatter.Field(memberNode));
        }
    }
}