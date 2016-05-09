namespace Skeleton.Core.Repository
{
    using Core.Domain;
    using System;
    using System.Linq.Expressions;

    public interface ISqlAggregateBuilder<TEntity, TIdentity>
     where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ISqlQueryBuilder<TEntity, TIdentity> SelectAverage(
           Expression<Func<TEntity, object>> expression);

        ISqlQueryBuilder<TEntity, TIdentity> SelectCount(
            Expression<Func<TEntity, object>> expression);

        ISqlQueryBuilder<TEntity, TIdentity> SelectMax(
            Expression<Func<TEntity, object>> expression);

        ISqlQueryBuilder<TEntity, TIdentity> SelectMin(
            Expression<Func<TEntity, object>> expression);

        ISqlQueryBuilder<TEntity, TIdentity> SelectSum(
            Expression<Func<TEntity, object>> expression);
    }
}
