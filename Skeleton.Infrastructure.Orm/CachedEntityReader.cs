using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;
using Skeleton.Abstraction.Reflection;
using Skeleton.Core;
using System;
using System.Collections.Generic;

namespace Skeleton.Infrastructure.Orm
{
    public sealed class CachedEntityReader<TEntity> :
            EntityReader<TEntity>,
            ICachedEntityReader<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        private readonly CacheKeyGenerator<TEntity> _keyGenerator;

        public CachedEntityReader(
            IMetadataProvider metadataProvider,
            IDatabase database,
            ICacheProvider cacheProvider)
            : base(metadataProvider, database)
        {
            cacheProvider.ThrowIfNull(nameof(cacheProvider));

            Cache = cacheProvider;
            _keyGenerator = new CacheKeyGenerator<TEntity>();
        }

        public Action<ICacheConfiguration> CacheConfigurator { get; set; }

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

        public override TEntity FirstOrDefault(object id)
        {
            id.ThrowIfNull(nameof(id));

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

        public override IEnumerable<TEntity> Query(IQuery query)
        {
            LastGeneratedCacheKey = _keyGenerator.ForFind(Builder.SqlQuery);

            return Cache.GetOrAdd(
                LastGeneratedCacheKey,
                () => base.Query(query),
                CacheConfigurator);
        }
    }
}