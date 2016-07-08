using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Core.Repository
{
    public interface IReadRepositoryAsync<TEntity, TIdentity> :
        IEntityRepository<TEntity, TIdentity>,
        IQueryAsync<TEntity, TIdentity>,
        IAggregateAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ISqlQuery SqlQuery { get; }

        IReadRepositoryAsync<TEntity, TIdentity> GroupBy(
            Expression<Func<TEntity, object>> expression);

        IReadRepositoryAsync<TEntity, TIdentity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IReadRepositoryAsync<TEntity, TIdentity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IReadRepositoryAsync<TEntity, TIdentity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IReadRepositoryAsync<TEntity, TIdentity> CrossJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IReadRepositoryAsync<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression);

        IReadRepositoryAsync<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression);

        IReadRepositoryAsync<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions);

        IReadRepositoryAsync<TEntity, TIdentity> SelectDistinct(
            Expression<Func<TEntity, object>> expression);

        IReadRepositoryAsync<TEntity, TIdentity> SelectTop(int take);

        IReadRepositoryAsync<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression);

        IReadRepositoryAsync<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            ISqlQuery sqlQuery);

        IReadRepositoryAsync<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);

        IReadRepositoryAsync<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            ISqlQuery sqlQuery);

        IReadRepositoryAsync<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);
    }
}