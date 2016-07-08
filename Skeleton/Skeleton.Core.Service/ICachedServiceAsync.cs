using Skeleton.Core.Repository;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Core.Service
{
    public interface ICachedServiceAsync<TEntity, TIdentity> :
        IEntityService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ICachedRepositoryAsync<TEntity, TIdentity> Repository { get; }
    }
}