namespace Skeleton.Core.Repository
{
    using Core.Domain;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    public interface ISqlQueryBuilder<TEntity, TIdentity>
       where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ISqlQueryBuilder<TEntity, TIdentity> And(
            Expression<Func<TEntity, bool>> expression);

        ISqlQuery AsSql();

        ISqlQueryBuilder<TEntity, TIdentity> GroupBy(
            Expression<Func<TEntity, object>> expression);

        ISqlQueryBuilder<TEntity2, TIdentity2> Join<TEntity2, TIdentity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity2>;

        ISqlQueryBuilder<TEntity, TIdentity> Or(
            Expression<Func<TEntity, bool>> expression);

        ISqlQueryBuilder<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression);

        ISqlQueryBuilder<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression);

        ISqlQueryBuilder<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions);

        ISqlQueryBuilder<TEntity, TIdentity> SelectDistinct(
            Expression<Func<TEntity, object>> expression);

        ISqlQueryBuilder<TEntity, TIdentity> SelectTop(int take);

        ISqlQueryBuilder<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression);

        ISqlQueryBuilder<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression, ISqlQuery sqlQuery);

        ISqlQueryBuilder<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression, IEnumerable<object> values);

        ISqlQueryBuilder<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression, ISqlQuery sqlQuery);

        ISqlQueryBuilder<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression, IEnumerable<object> values);

        ISqlQueryBuilder<TEntity, TIdentity> WherePrimaryKey(
            Expression<Func<TEntity, bool>> expression);

        ISqlAggregateBuilder<TEntity,TIdentity> Aggregate();
    }
}