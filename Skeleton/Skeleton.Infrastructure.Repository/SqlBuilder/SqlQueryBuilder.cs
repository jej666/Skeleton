//https://github.com/base33/lambda-sql-builder
namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    using Common.Extensions;
    using Common.Reflection;
    using Core.Domain;
    using Core.Repository;
    using ExpressionTree;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    public sealed class SqlQueryBuilder<TEntity, TIdentity> :
        SqlBuilderBase,
        ISqlQueryBuilder<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ITypeAccessor _accessor;

        public SqlQueryBuilder(ITypeAccessorCache accessorCache)
            : base(TableInfo.GetTableName<TEntity>())
        {
            accessorCache.ThrowIfNull(() => accessorCache);

            _accessor = accessorCache.Get<TEntity>();
        }

        private SqlQueryBuilder(InternalQueryBuilder builder, LambdaExpressionResolver resolver)
            : base(builder, resolver)
        { }

        public ISqlQueryBuilder<TEntity, TIdentity> And(Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Resolver.ResolveQuery(expression);

            return this;
        }

        public ISqlQuery AsSql()
        {
            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> GroupBy(Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Resolver.GroupBy(expression);

            return this;
        }

        public ISqlQueryBuilder<TEntity2, TIdentity2> Join<TEntity2, TIdentity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity2>
        {
            expression.ThrowIfNull(() => expression);

            var joinQuery = new SqlQueryBuilder<TEntity2, TIdentity2>(Builder, Resolver);
            Resolver.Join(expression);

            return joinQuery;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> Or(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);

            Builder.Or();
            Resolver.ResolveQuery(expression);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Resolver.OrderBy(expression);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Resolver.OrderByDescending(expression);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions)
        {
            expressions.ThrowIfNull(() => expressions);

            foreach (var expression in expressions)
                Resolver.Select(expression);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> SelectAverage(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Resolver.SelectWithFunction(expression, SelectFunction.AVG);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> SelectCount(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Resolver.SelectWithFunction(expression, SelectFunction.COUNT);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> SelectDistinct(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Resolver.SelectWithFunction(expression, SelectFunction.DISTINCT);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> SelectMax(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Resolver.SelectWithFunction(expression, SelectFunction.MAX);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> SelectMin(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Resolver.SelectWithFunction(expression, SelectFunction.MIN);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> SelectSum(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Resolver.SelectWithFunction(expression, SelectFunction.SUM);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> SelectTop(int take)
        {
            Resolver.SelectTop(take);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            return And(expression);
        }

        public ISqlQueryBuilder<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression, ISqlQuery sqlQuery)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Resolver.QueryByIsIn(expression, sqlQuery);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression, IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Resolver.QueryByIsIn(expression, values);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression, ISqlQuery sqlQuery)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Resolver.QueryByNotIn(expression, sqlQuery);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Resolver.QueryByNotIn(expression, values);

            return this;
        }

        public ISqlQueryBuilder<TEntity, TIdentity> WherePrimaryKey(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            var instance = _accessor.CreateInstance<TEntity>();

            Builder.And();
            Resolver.QueryByPrimaryKey(instance.IdAccessor.Name, expression);

            return this;
        }
    }
}

//public ISqlQueryBuilder<TResult> Join<T2, TKey, TResult>(SqlBuilder<T2> joinQuery,
//    Expression<Func<T, TKey>> primaryKeySelector,
//    Expression<Func<T, TKey>> foreignKeySelector,
//    Func<T, T2, TResult> selection)
//{
//    var query = new SqlBuilder<TResult>(Builder, Resolver);
//    Resolver.Join<T, T2, TKey>(primaryKeySelector, foreignKeySelector);
//    return query;
//}