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

    public abstract class CachedRepositoryBase<TEntity, TIdentity> :
        ReadOnlyRepositoryBase<TEntity, TIdentity>,
        ICachedRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly CacheKeyGenerator<TEntity, TIdentity> _keyGenerator =
            new CacheKeyGenerator<TEntity, TIdentity>();

        protected CachedRepositoryBase(
            ITypeAccessorCache accessorCache,
            ICacheProvider cacheProvider,
            IDatabase database)
            : base(accessorCache, database)
        {
            cacheProvider.ThrowIfNull(() => cacheProvider);

            _cacheProvider = cacheProvider;
        }

        protected CachedRepositoryBase(
           ITypeAccessorCache typeAccessorCache,
           ICacheProvider cacheProvider,
           IDatabaseFactory databaseFactory,
           Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
            : this(
            typeAccessorCache,
            cacheProvider,
            databaseFactory.CreateDatabase(configurator))
        { }

        public ICacheProvider Cache
        {
            get { return _cacheProvider; }
        }

        protected Action<ICacheContext> CacheConfigurator { get; set; }

        public override IEnumerable<TEntity> Find(
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy)
        {
            where.ThrowIfNull(() => where);
            orderBy.ThrowIfNull(() => orderBy);

            var sql = Query.Where(where)
                                  .OrderBy(orderBy)
                                  .AsSql();

            var key = _keyGenerator.ForFind(sql);
            Func<IEnumerable<TEntity>> valueFactory = () =>
              Database.Find<TEntity>(
                     sql.Query,
                     sql.Parameters);

            return Cache.GetOrAdd(key, valueFactory, CacheConfigurator);
        }

        public override TEntity FirstOrDefault(TIdentity id)
        {
            id.ThrowIfNull(() => id);

            var key = _keyGenerator.ForFirstOrDefault(id);
            Func<TEntity> valueFactory = () => base.FirstOrDefault(id);

            return Cache.GetOrAdd(key, valueFactory, CacheConfigurator);
        }

        public override TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where)
        {
            where.ThrowIfNull(() => where);

            var sql = Query.Where(where).AsSql();
            var key = _keyGenerator.ForFirstOrDefault(sql.Parameters);
            Func<TEntity> valueFactory = () =>
                Database.FirstOrDefault<TEntity>(sql.Query, sql.Parameters);

            return Cache.GetOrAdd(key, valueFactory, CacheConfigurator);
        }

        public override IEnumerable<TEntity> GetAll()
        {
            var key = _keyGenerator.ForGetAll();
            Func<IEnumerable<TEntity>> valueFactory = () => base.GetAll();

            return Cache.GetOrAdd(key, valueFactory, CacheConfigurator);
        }

        public override IEnumerable<TEntity> PageAll(int pageSize, int pageNumber)
        {
            var key = _keyGenerator.ForPageAll(pageSize, pageNumber);
            Func<IEnumerable<TEntity>> valueFactory = () =>
                base.PageAll(pageSize, pageNumber);

            return Cache.GetOrAdd(key, valueFactory, CacheConfigurator);
        }

        public override IEnumerable<TEntity> Page(
            int pageSize,
            int pageNumber,
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy)
        {
            where.ThrowIfNull(() => where);
            orderBy.ThrowIfNull(() => orderBy);

            var sql = Query.Where(where)
                                  .OrderBy(orderBy)
                                  .AsSql();
            var key = _keyGenerator.ForPage(
                pageSize, pageNumber, sql.Parameters);
            Func<IEnumerable<TEntity>> valueFactory = () =>
               Database.Find<TEntity>(
                    sql.PagedQuery(pageSize, pageNumber),
                    sql.Parameters);

            return Cache.GetOrAdd(key, valueFactory, CacheConfigurator);
        }
    }
}