using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Skeleton.Core.Domain;

namespace Skeleton.Core.Repository
{
    public interface IReadOnlyRepositoryAsync<TEntity, TIdentity> :
        IEntityRepository<TEntity, TIdentity>,
        IQueryAsync<TEntity, TIdentity>,
        IAggregateAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ISqlQuery SqlQuery { get; }

        IReadOnlyRepositoryAsync<TEntity, TIdentity> GroupBy(
            Expression<Func<TEntity, object>> expression);

        IReadOnlyRepositoryAsync<TEntity, TIdentity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IReadOnlyRepositoryAsync<TEntity, TIdentity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IReadOnlyRepositoryAsync<TEntity, TIdentity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IReadOnlyRepositoryAsync<TEntity, TIdentity> CrossJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IReadOnlyRepositoryAsync<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression);

        IReadOnlyRepositoryAsync<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression);

        IReadOnlyRepositoryAsync<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions);

        IReadOnlyRepositoryAsync<TEntity, TIdentity> SelectDistinct(
            Expression<Func<TEntity, object>> expression);

        IReadOnlyRepositoryAsync<TEntity, TIdentity> SelectTop(int take);

        IReadOnlyRepositoryAsync<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression);

        IReadOnlyRepositoryAsync<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            ISqlQuery sqlQuery);

        IReadOnlyRepositoryAsync<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);

        IReadOnlyRepositoryAsync<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            ISqlQuery sqlQuery);

        IReadOnlyRepositoryAsync<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);
    }
}