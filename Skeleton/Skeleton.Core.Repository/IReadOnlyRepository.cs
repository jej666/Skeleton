namespace Skeleton.Core.Repository
{
    using Common;
    using Core.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IReadOnlyRepository<TEntity, TIdentity> :
        IDisposable,
        IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IQueryBuilder<TEntity, TIdentity> Query { get; }

        IAggregateBuilder<TEntity, TIdentity> Aggregate { get; }

        TEntity FirstOrDefault(TIdentity id);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where);

        IEnumerable<TEntity> Find(
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy); 

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> PageAll(int pageSize, int pageNumber);

        IEnumerable<TEntity> Page(
            int pageSize,
            int pageNumber,
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy);
    }
}