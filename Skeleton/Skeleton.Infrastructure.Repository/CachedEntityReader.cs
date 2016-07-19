using System;
using System.Collections.Generic;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;

namespace Skeleton.Infrastructure.Repository
{
    public class CachedEntityReader<TEntity, TIdentity> :
        EntityReader<TEntity, TIdentity>,
        ICachedEntityReader<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly CacheKeyGenerator<TEntity, TIdentity> _keyGenerator;

        public CachedEntityReader(
            IMetadataProvider metadataProvider,
            IDatabase database,
            ICacheProvider cacheProvider)
            : base(metadataProvider, database)
        {
            cacheProvider.ThrowIfNull(() => cacheProvider);

            _cacheProvider = cacheProvider;
            _keyGenerator = new CacheKeyGenerator<TEntity, TIdentity>(isAsync: false);
        }

        public Action<ICacheContext> CacheConfigurator { get; set; }

        public ICacheProvider Cache
        {
            get { return _cacheProvider; }
        }

        public string LastGeneratedCacheKey { get; private set; }

        public override IEnumerable<TEntity> Find()
        {
            LastGeneratedCacheKey = _keyGenerator.ForFind(Builder.SelectQuery);

            return Cache.GetOrAdd(
                LastGeneratedCacheKey,
                () => base.Find(),
                CacheConfigurator);
        }

        public override TEntity FirstOrDefault(TIdentity id)
        {
            id.ThrowIfNull(() => id);

            LastGeneratedCacheKey = _keyGenerator.ForFirstOrDefault(id);

            return Cache.GetOrAdd(
                LastGeneratedCacheKey,
                () => base.FirstOrDefault(id),
                CacheConfigurator);
        }

        public override TEntity FirstOrDefault()
        {
            LastGeneratedCacheKey = _keyGenerator.ForFirstOrDefault(Builder.SelectQuery);

            return Cache.GetOrAdd(
                LastGeneratedCacheKey,
                () => base.FirstOrDefault(),
                CacheConfigurator);
        }

        public override IEnumerable<TEntity> GetAll()
        {
            LastGeneratedCacheKey = _keyGenerator.ForGetAll();

            return Cache.GetOrAdd(
                LastGeneratedCacheKey,
                () => base.GetAll(),
                CacheConfigurator);
        }

        public override IEnumerable<TEntity> Page(int pageSize, int pageNumber)
        {
            LastGeneratedCacheKey = _keyGenerator.ForPage(pageSize, pageNumber);

            return Cache.GetOrAdd(
                LastGeneratedCacheKey,
                () => base.Page(pageSize, pageNumber),
                CacheConfigurator);
        }
    }
}