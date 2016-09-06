using System;
using System.Collections.Generic;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class CachedEntityReader<TEntity, TIdentity> :
            EntityReader<TEntity, TIdentity>,
            ICachedEntityReader<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly CacheKeyGenerator<TEntity, TIdentity> _keyGenerator;

        public CachedEntityReader(
            IMetadataProvider metadataProvider,
            IDatabase database,
            ICacheProvider cacheProvider)
            : base(metadataProvider, database)
        {
            cacheProvider.ThrowIfNull(() => cacheProvider);

            Cache = cacheProvider;
            _keyGenerator = new CacheKeyGenerator<TEntity, TIdentity>();
        }

        public Action<ICacheContext> CacheConfigurator { get; set; }

        public ICacheProvider Cache { get; }

        public string LastGeneratedCacheKey { get; private set; }

        public override IEnumerable<TEntity> Find()
        {
            LastGeneratedCacheKey = _keyGenerator.ForFind(Builder.SqlQuery);

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
            LastGeneratedCacheKey = _keyGenerator.ForFirstOrDefault(Builder.SqlQuery);

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