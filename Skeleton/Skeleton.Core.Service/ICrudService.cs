using Skeleton.Abstraction;
using Skeleton.Core.Repository;

namespace Skeleton.Core.Service
{
    public interface ICrudService<TEntity, TIdentity> :
        IEntityService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ICrudRepository<TEntity, TIdentity> Repository { get; }
    }
}