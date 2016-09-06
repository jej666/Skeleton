﻿using System;

namespace Skeleton.Abstraction.Repository
{
    public interface ICachedEntityReader<TEntity, TIdentity> :
            IEntityReader<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ICacheProvider Cache { get; }

        string LastGeneratedCacheKey { get; }

        Action<ICacheContext> CacheConfigurator { get; set; }
    }
}