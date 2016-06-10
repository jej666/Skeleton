using Skeleton.Abstraction;
using Skeleton.Core.Repository;

namespace Skeleton.Core.Service
{
    public interface IReadOnlyService<TEntity, TIdentity> :
        IEntityService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IReadOnlyRepository<TEntity, TIdentity> Repository { get; }
    }
}