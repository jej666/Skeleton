namespace Skeleton.Core.Repository
{
    using Core.Domain;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepositoryAsync<TEntity, TIdentity> :
        IReadOnlyRepositoryAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        Task<bool> AddAsync(TEntity entity);

        Task<bool> AddAsync(IEnumerable<TEntity> entities);

        Task<bool> DeleteAsync(TEntity entity);

        Task<bool> DeleteAsync(IEnumerable<TEntity> entities);

        Task<bool> SaveAsync(TEntity entity);

        Task<bool> SaveAsync(IEnumerable<TEntity> entities);

        Task<bool> UpdateAsync(TEntity entity);

        Task<bool> UpdateAsync(IEnumerable<TEntity> entities);
    }
}