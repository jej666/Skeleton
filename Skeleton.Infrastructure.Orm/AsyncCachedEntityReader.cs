﻿using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;
using Skeleton.Abstraction.Reflection;
using Skeleton.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Orm
{
    public sealed class AsyncCachedEntityReader<TEntity> :
            AsyncEntityReader<TEntity>,
            IAsyncCachedEntityReader<TEntity>
        where TEntity : class, IEntity<TEntity>, new()
    {
        private readonly CacheKeyGenerator<TEntity> _keyGenerator =
            new CacheKeyGenerator<TEntity>();

        public AsyncCachedEntityReader(
            IMetadataProvider metadataProvider,
            IAsyncDatabase database,
            IAsyncCacheProvider cacheProvider)
            : base(metadataProvider, database)
        {
            Cache = cacheProvider;
        }

        public Action<ICacheConfiguration> CacheConfigurator { get; set; }

        public IAsyncCacheProvider Cache { get; }

        public string LastGeneratedCacheKey { get; private set; }

        public override async Task<IEnumerable<TEntity>> FindAsync()
        {
            LastGeneratedCacheKey = _keyGenerator.ForFind(Builder.SqlQuery);

            return await Cache.GetOrAddAsync(
                    LastGeneratedCacheKey,
                    () => base.FindAsync(),
                    CacheConfigurator)
                .ConfigureAwait(false);
        }

        public override async Task<TEntity> FirstOrDefaultAsync(object id)
        {
            id.ThrowIfNull();

            LastGeneratedCacheKey = _keyGenerator.ForFirstOrDefault(id);

            return await Cache.GetOrAddAsync(
                    LastGeneratedCacheKey,
                    () => base.FirstOrDefaultAsync(id),
                    CacheConfigurator)
                .ConfigureAwait(false);
        }

        public override async Task<TEntity> FirstOrDefaultAsync()
        {
            LastGeneratedCacheKey = _keyGenerator.ForFirstOrDefault(Builder.SqlQuery);

            return await Cache.GetOrAddAsync(
                    LastGeneratedCacheKey,
                    () => base.FirstOrDefaultAsync(),
                    CacheConfigurator)
                .ConfigureAwait(false);
        }

        public override async Task<IEnumerable<TEntity>> QueryAsync(IQuery query)
        {
            LastGeneratedCacheKey = _keyGenerator.ForFind(Builder.SqlQuery);

            return await Cache.GetOrAddAsync(
                    LastGeneratedCacheKey,
                    () => base.QueryAsync(query),
                    CacheConfigurator)
                .ConfigureAwait(false);
        }
    }
}