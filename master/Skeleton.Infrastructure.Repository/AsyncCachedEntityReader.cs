using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class AsyncCachedEntityReader<TEntity> :
            AsyncEntityReader<TEntity>,
            IAsyncCachedEntityReader<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        private readonly AsyncCacheKeyGenerator<TEntity> _keyGenerator;

        public AsyncCachedEntityReader(
            IMetadataProvider metadataProvider,
            IDatabaseAsync database,
            ICacheProvider cacheProvider)
            : base(metadataProvider, database)
        {
            Cache = cacheProvider;
            _keyGenerator = new AsyncCacheKeyGenerator<TEntity>();
        }

        public Action<ICacheContext> CacheConfigurator { get; set; }

        public ICacheProvider Cache { get; }

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
            id.ThrowIfNull(() => id);

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

        public override async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            LastGeneratedCacheKey = _keyGenerator.ForGetAll();

            return await Cache.GetOrAddAsync(
                    LastGeneratedCacheKey,
                    () => base.GetAllAsync(),
                    CacheConfigurator)
                .ConfigureAwait(false);
        }

        public override async Task<IEnumerable<TEntity>> PageAsync(
            int pageSize,
            int pageNumber)
        {
            LastGeneratedCacheKey = _keyGenerator.ForPage(pageSize, pageNumber);

            return await Cache.GetOrAddAsync(
                    LastGeneratedCacheKey,
                    () => base.PageAsync(pageSize, pageNumber),
                    CacheConfigurator)
                .ConfigureAwait(false);
        }
    }
}