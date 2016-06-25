using Skeleton.Abstraction;
using Skeleton.Core.Repository;

namespace Skeleton.Core.Service
{
    public interface ICrudServiceAsync<TEntity, TIdentity> :
        IEntityService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ICrudRepositoryAsync<TEntity, TIdentity> Repository { get; }
    }
}