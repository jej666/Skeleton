using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;

namespace Skeleton.Infrastructure.Repository
{
    public class AsyncCachedEntityReader<TEntity, TIdentity> :
        AsyncEntityReader<TEntity, TIdentity>,
        IAsyncCachedEntityReader<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly CacheKeyGenerator<TEntity, TIdentity> _keyGenerator;

        public AsyncCachedEntityReader(
            IMetadataProvider metadataProvider,
            IDatabaseAsync database,
            ICacheProvider cacheProvider)
            : base(metadataProvider, database)
        {
            _cacheProvider = cacheProvider;
            _keyGenerator = new CacheKeyGenerator<TEntity, TIdentity>(isAsync:true);
        }

        public Action<ICacheContext> CacheConfigurator { get; set; }

        public ICacheProvider Cache
        {
            get { return _cacheProvider; }
        }

        public string LastGeneratedCacheKey { get; private set; }

        public override async Task<IEnumerable<TEntity>> FindAsync()
        {
            LastGeneratedCacheKey = _keyGenerator.ForFind(Builder.Query);

            return await Cache.GetOrAddAsync(
                LastGeneratedCacheKey,
                () => base.FindAsync(),
                CacheConfigurator)
                .ConfigureAwait(false);
        }

        public override async Task<TEntity> FirstOrDefaultAsync(TIdentity id)
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
            LastGeneratedCacheKey = _keyGenerator.ForFirstOrDefault(Builder.Query);

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