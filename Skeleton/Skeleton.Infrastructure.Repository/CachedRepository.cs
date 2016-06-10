using Skeleton.Abstraction;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using System;
using System.Collections.Generic;

namespace Skeleton.Infrastructure.Repository
{
    public class CachedRepository<TEntity, TIdentity> :
        ReadOnlyRepository<TEntity, TIdentity>,
        ICachedRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ICacheProvider _cacheProvider;

        private readonly CacheKeyGenerator<TEntity, TIdentity> _keyGenerator =
            new CacheKeyGenerator<TEntity, TIdentity>();

        public CachedRepository(
            ITypeAccessorCache accessorCache,
            ICacheProvider cacheProvider,
            IDatabase database)
            : base(accessorCache, database)
        {
            cacheProvider.ThrowIfNull(() => cacheProvider);

            _cacheProvider = cacheProvider;
        }

        public Action<ICacheContext> CacheConfigurator { get; protected set; }

        public ICacheProvider Cache
        {
            get { return _cacheProvider; }
        }

        public ICacheKeyGenerator<TEntity, TIdentity> CacheKeyGenerator
        {
            get { return _keyGenerator; }
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
                _keyGenerator.ForPage(pageSize, pageNumber),
                () => base.Page(pageSize, pageNumber),
                CacheConfigurator);
        }
    }
}