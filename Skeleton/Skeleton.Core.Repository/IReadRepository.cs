using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Skeleton.Abstraction;

namespace Skeleton.Core.Repository
{
    public interface IReadRepository<TEntity, TIdentity> :
        IEntityRepository<TEntity, TIdentity>,
        IQuery<TEntity, TIdentity>,
        IAggregate<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ISqlQuery SqlQuery { get; }

        IReadRepository<TEntity, TIdentity> GroupBy(
            Expression<Func<TEntity, object>> expression);

        IReadRepository<TEntity, TIdentity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IReadRepository<TEntity, TIdentity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IReadRepository<TEntity, TIdentity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IReadRepository<TEntity, TIdentity> CrossJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IReadRepository<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression);

        IReadRepository<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression);

        IReadRepository<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions);

        IReadRepository<TEntity, TIdentity> SelectDistinct(
            Expression<Func<TEntity, object>> expression);

        IReadRepository<TEntity, TIdentity> SelectTop(int take);

        IReadRepository<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression);

        IReadRepository<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            ISqlQuery sqlQuery);

        IReadRepository<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);

        IReadRepository<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            ISqlQuery sqlQuery);

        IReadRepository<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);
    }
}