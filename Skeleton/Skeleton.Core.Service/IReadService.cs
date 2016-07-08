using Skeleton.Core.Repository;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Core.Service
{
    public interface IReadService<TEntity, TIdentity> :
        IEntityService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IReadRepository<TEntity, TIdentity> Repository { get; }
    }
}