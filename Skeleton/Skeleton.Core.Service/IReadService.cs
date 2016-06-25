using Skeleton.Abstraction;
using Skeleton.Core.Repository;

namespace Skeleton.Core.Service
{
    public interface IReadService<TEntity, TIdentity> :
        IEntityService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IReadRepository<TEntity, TIdentity> Repository { get; }
    }
}