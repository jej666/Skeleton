using System;

namespace Skeleton.Abstraction.Repository
{
    public interface IAsyncCachedEntityReader<TEntity, TIdentity> :
            IAsyncEntityReader<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ICacheProvider Cache { get; }

        string LastGeneratedCacheKey { get; }

        Action<ICacheContext> CacheConfigurator { get; set; }
    }
}