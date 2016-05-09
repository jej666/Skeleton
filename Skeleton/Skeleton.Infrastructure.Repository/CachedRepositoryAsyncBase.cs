namespace Skeleton.Infrastructure.Repository
{
    using Common;
    using Common.Extensions;
    using Common.Reflection;
    using Core.Domain;
    using Core.Repository;
    using Data;
    using Data.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

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
        { }

        public ICacheProvider Cache
        {
            get { return _cacheProvider; }
        }

        protected Action<ICacheContext> CacheConfigurator { get; set; }

        public async override Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy)
        {
            where.ThrowIfNull(() => where);
            orderBy.ThrowIfNull(() => orderBy);

            var sql = QueryBuilder.Where(where)
                                  .OrderBy(orderBy)
                                  .AsSql();

            return await FindAsync(sql);
        }

        public override async Task<IEnumerable<TEntity>> FindAsync(
            ISqlQuery query)
        {
            query.ThrowIfNull(() => query);
            var key = _keyGenerator.ForFind(query);

            return await Cache.GetOrAddAsync(key, () =>
                base.FindAsync(query),
                CacheConfigurator)
                .ConfigureAwait(false);
        }

        public  override async Task<TEntity> FirstOrDefaultAsync(
            TIdentity id)
        {
            id.ThrowIfNull(() => id);

            var key = _keyGenerator.ForFirstOrDefault(id);

            return await Cache.GetOrAddAsync<TEntity>(key,
                () => base.FirstOrDefaultAsync(id),
                CacheConfigurator)
                .ConfigureAwait(false);
        }

        public  override async Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> where)
        {
            where.ThrowIfNull(() => where);

            var sql = QueryBuilder.Where(where).AsSql();
            var key = _keyGenerator.ForFirstOrDefault(sql.Parameters);

            return await Cache.GetOrAddAsync(key, () =>
                Database.FirstOrDefaultAsync<TEntity>(sql.Query, sql.Parameters),
                CacheConfigurator)
                .ConfigureAwait(false);
        }

        public async override Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var key = _keyGenerator.ForGetAll();

            return await Cache.GetOrAddAsync(key, () =>
                base.GetAllAsync(),
                CacheConfigurator);
        }

        public async override Task<IEnumerable<TEntity>> PageAllAsync(
            int pageSize, 
            int pageNumber)
        {
            var key = _keyGenerator.ForPageAll(pageSize, pageNumber);

            return await Cache.GetOrAddAsync(key,
                () => base.PageAllAsync(pageSize, pageNumber),
                CacheConfigurator)
                .ConfigureAwait(false);
        }

        public async override Task<IEnumerable<TEntity>> PageAsync(
            int pageSize,
            int pageNumber,
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy)
        {
            where.ThrowIfNull(() => where);
            orderBy.ThrowIfNull(() => orderBy);

            var sql = QueryBuilder.Where(where)
                                  .OrderBy(orderBy)
                                  .AsSql();

            var key = _keyGenerator.ForPage(
                pageSize, pageNumber, sql.Parameters);

            return await Cache.GetOrAddAsync(key, () =>
                Database.FindAsync<TEntity>(
                    sql.PagedQuery(pageSize, pageNumber),
                    sql.Parameters),
                CacheConfigurator)
                .ConfigureAwait(false);
        }
    }
}