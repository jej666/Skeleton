using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Abstraction.Repository
{
    public interface IAsyncEntityPersistor<in TEntity> :
            IDisposable,
            IHideObjectMethods
        where TEntity : class, IEntity<TEntity>
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