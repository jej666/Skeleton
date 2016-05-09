namespace Skeleton.Core.Repository
{
    using Common;
    using Core.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IReadOnlyRepositoryAsync<TEntity, TIdentity> :
        IDisposable,
        IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ISqlQueryBuilder<TEntity, TIdentity> QueryBuilder { get; }

        Task<TEntity> FirstOrDefaultAsync(TIdentity id);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where);

        Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy);

        Task<IEnumerable<TEntity>> FindAsync(ISqlQuery query);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> PageAllAsync(int pageSize, int pageNumber);

        Task<IEnumerable<TEntity>> PageAsync(
            int pageSize,
            int pageNumber,
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy);
    }
}