using Skeleton.Abstraction;
using Skeleton.Abstraction.Reflection;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Repository
{
    public class CachedRepositoryAsync<TEntity, TIdentity> :
        ReadRepositoryAsync<TEntity, TIdentity>,
        ICachedRepositoryAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly CacheKeyGenerator<TEntity, TIdentity> _keyGenerator;

        public CachedRepositoryAsync(
            IMetadataProvider metadataProvider,
            IDatabaseAsync database,
            ICacheProvider cacheProvider)
            : base(metadataProvider, database)
        {
            cacheProvider.ThrowIfNull(() => cacheProvider);

            _cacheProvider = cacheProvider;
            _keyGenerator = new CacheKeyGenerator<TEntity, TIdentity>();
        }

        public Action<ICacheContext> CacheConfigurator { get; set; }

        public ICacheProvider Cache
        {
            get { return _cacheProvider; }
        }

        public ICacheKeyGenerator<TEntity, TIdentity> CacheKeyGenerator
        {
            get { return _keyGenerator; }
        }

        public override async Task<IEnumerable<TEntity>> FindAsync()
        {
            return await Cache.GetOrAddAsync(
                _keyGenerator.ForFind(SqlQuery),
                () => base.FindAsync(),
                CacheConfigurator)
                .ConfigureAwait(false);
        }

        public override async Task<TEntity> FirstOrDefaultAsync(TIdentity id)
        {
            id.ThrowIfNull(() => id);

            return await Cache.GetOrAddAsync(
                _keyGenerator.ForFirstOrDefault(id),
                () => base.FirstOrDefaultAsync(id),
                CacheConfigurator)
                .ConfigureAwait(false);
        }

        public override async Task<TEntity> FirstOrDefaultAsync()
        {
            return await Cache.GetOrAddAsync(
                _keyGenerator.ForFirstOrDefault(SqlQuery),
                () => base.FirstOrDefaultAsync(),
                CacheConfigurator)
                .ConfigureAwait(false);
        }

        public override async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Cache.GetOrAddAsync(
                _keyGenerator.ForGetAll(),
                () => base.GetAllAsync(),
                CacheConfigurator)
                .ConfigureAwait(false);
        }

        public override async Task<IEnumerable<TEntity>> PageAsync(
            int pageSize,
            int pageNumber)
        {
            return await Cache.GetOrAddAsync(
                _keyGenerator.ForPage(pageSize, pageNumber),
                () => base.PageAsync(pageSize, pageNumber),
                CacheConfigurator)
                .ConfigureAwait(false);
        }
    }
}