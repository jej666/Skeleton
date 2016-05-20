using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Skeleton.Common;
using Skeleton.Common.Extensions;
using Skeleton.Common.Reflection;
using Skeleton.Core.Domain;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Data.Configuration;

namespace Skeleton.Infrastructure.Repository
{
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
        {
        }

        protected Action<ICacheContext> CacheConfigurator { get; set; }

        public ICacheProvider Cache
        {
            get { return _cacheProvider; }
        }

        public override IEnumerable<TEntity> Find()
        {
            return Cache.GetOrAdd(
                _keyGenerator.ForFind(SqlQuery), 
                () => base.Find(), 
                CacheConfigurator);
        }

        public override TEntity FirstOrDefault(TIdentity id)
        {
            id.ThrowIfNull(() => id);

            return Cache.GetOrAdd(
                _keyGenerator.ForFirstOrDefault(id), 
                () => base.FirstOrDefault(id), 
                CacheConfigurator);
        }

        public override TEntity FirstOrDefault()
        {
            return Cache.GetOrAdd(
                _keyGenerator.ForFirstOrDefault(SqlQuery), 
                () => base.FirstOrDefault(), 
                CacheConfigurator);
        }

        public override IEnumerable<TEntity> GetAll()
        {
            return Cache.GetOrAdd(
                _keyGenerator.ForGetAll(), 
                () => base.GetAll(), 
                CacheConfigurator);
        }

        public override IEnumerable<TEntity> Page(int pageSize, int pageNumber)
        {
            return Cache.GetOrAdd(
                _keyGenerator.ForPageAll(pageSize, pageNumber), 
                () => base.Page(pageSize, pageNumber),
                CacheConfigurator);
        }
    }
}