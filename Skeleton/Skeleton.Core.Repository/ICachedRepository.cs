using System;
using Skeleton.Abstraction;

namespace Skeleton.Core.Repository
{
    public interface ICachedRepository<TEntity, TIdentity> :
        IReadOnlyRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ICacheProvider Cache { get; }

        ICacheKeyGenerator<TEntity, TIdentity> CacheKeyGenerator { get; }

        Action<ICacheContext> CacheConfigurator { get; set; }
    }
}