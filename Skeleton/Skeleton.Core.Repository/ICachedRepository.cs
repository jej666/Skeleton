using System;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Core.Repository
{
    public interface ICachedRepository<TEntity, TIdentity> :
        IReadRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ICacheProvider Cache { get; }

        ICacheKeyGenerator<TEntity, TIdentity> CacheKeyGenerator { get; }

        Action<ICacheContext> CacheConfigurator { get; set; }
    }
}