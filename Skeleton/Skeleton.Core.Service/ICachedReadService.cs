using Skeleton.Core.Repository;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Core.Service
{
    public interface ICachedReadService<TEntity, TIdentity, TDto> :
        IReadService<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        new ICachedEntityReader<TEntity, TIdentity> Query { get; }
    }
}