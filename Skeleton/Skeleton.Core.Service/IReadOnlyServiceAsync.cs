using Skeleton.Abstraction;
using Skeleton.Core.Repository;

namespace Skeleton.Core.Service
{
    public interface IReadOnlyServiceAsync<TEntity, TIdentity> :
        IEntityService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IReadOnlyRepositoryAsync<TEntity, TIdentity> Repository { get; }
    }
}