using Skeleton.Core.Repository;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Core.Service
{
    public interface IAsyncCrudService<TEntity, TIdentity, TDto> :
        IAsyncReadService<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        IAsyncEntityPersistor<TEntity, TIdentity> Store { get; }
    }
}