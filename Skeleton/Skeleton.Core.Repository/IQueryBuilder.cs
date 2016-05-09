namespace Skeleton.Core.Repository
{
    using Core.Domain;
    using Common;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IQueryBuilder<TEntity, TIdentity>:
        IHideObjectMethods,
        IQueryWhereBuilder<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ISqlQuery AsSql();

        IEnumerable<TEntity> Find();

        TEntity FirstOrDefault();

        IQueryBuilder<TEntity, TIdentity> GroupBy(
            Expression<Func<TEntity, object>> expression);

        IQueryBuilder<TEntity2, TIdentity2> Join<TEntity2, TIdentity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity2>;

        IQueryBuilder<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression);

        IQueryBuilder<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression);

        IQueryBuilder<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions);

        IQueryBuilder<TEntity, TIdentity> SelectDistinct(
            Expression<Func<TEntity, object>> expression);

        IQueryBuilder<TEntity, TIdentity> SelectTop(int take);
    }
}