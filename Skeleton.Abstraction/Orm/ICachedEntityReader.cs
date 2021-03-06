﻿using Skeleton.Abstraction.Domain;
using System;

namespace Skeleton.Abstraction.Orm
{
    public interface ICachedEntityReader<TEntity> :
            IEntityReader<TEntity>
        where TEntity : class, IEntity<TEntity>, new()
    {
        ICacheProvider Cache { get; }

        string LastGeneratedCacheKey { get; }

        Action<ICacheConfiguration> CacheConfigurator { get; set; }
    }
}