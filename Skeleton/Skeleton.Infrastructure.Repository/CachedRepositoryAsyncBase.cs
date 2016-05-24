using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skeleton.Common;
using Skeleton.Common.Extensions;
using Skeleton.Common.Reflection;
using Skeleton.Core.Domain;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Data.Configuration;

namespace Skeleton.Infrastructure.Repository
{
    public abstract class CachedRepositoryAsyncBase<TEntity, TIdentity> :
        ReadOnlyRepositoryAsyncBase<TEntity, TIdentity>,
        ICachedRepositoryAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ICacheProvider _cacheProvider;

        private readonly CacheKeyGenerator<TEntity, TIdentity> _keyGenerator =
            new CacheKeyGenerator<TEntity, TIdentity>();

        protected CachedRepositoryAsyncBase(
            ITypeAccessorCache accessorCache,
            ICacheProvider cacheProvider,
            IDatabaseAsync database)
            : base(accessorCache, database)
        {
            cacheProvider.ThrowIfNull(() => cacheProvider);

            _cacheProvider = cacheProvider;
        }

        protected CachedRepositoryAsyncBase(
            ITypeAccessorCache typeAccessorCache,
            ICacheProvider cacheProvider,
            IDatabaseFactory databaseFactory,
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
            : this(
                typeAccessorCache,
                cacheProvider,
                databaseFactory.CreateDatabaseForAsyncOperations(configurator))
        {
        }

        protected Action<ICacheContext> CacheConfigurator { get; set; }

        public ICacheProvider Cache
        {
            get { return _cacheProvider; }
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
                _keyGenerator.ForPageAll(pageSize, pageNumber),
                () => base.PageAsync(pageSize, pageNumber),
                CacheConfigurator)
                .ConfigureAwait(false);
        }
    }
}