using Skeleton.Abstraction.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Abstraction.Orm
{
    public interface IAsyncEntityWriter<in TEntity> :
            IDisposable,
            IHideObjectMethods
        where TEntity : class, IEntity<TEntity>, new()
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