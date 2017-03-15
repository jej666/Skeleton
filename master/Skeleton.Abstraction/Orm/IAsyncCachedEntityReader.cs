using Skeleton.Abstraction.Domain;
using System;

namespace Skeleton.Abstraction.Orm
{
    public interface IAsyncCachedEntityReader<TEntity> :
            IAsyncEntityReader<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        ICacheProvider Cache { get; }

        string LastGeneratedCacheKey { get; }

        Action<ICacheContext> CacheConfigurator { get; set; }
    }
}