using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Reflection;
using Skeleton.Abstraction.Orm;
using Skeleton.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Orm
{
    public sealed class AsyncCachedEntityReader<TEntity> :
            AsyncEntityReader<TEntity>,
            IAsyncCachedEntityReader<TEntity>
        where TEntity : class, IEntity<TEntity>
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
            id.ThrowIfNull(nameof(id));

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
    }
}