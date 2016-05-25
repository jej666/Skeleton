using System;
using Skeleton.Common;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Tests.Infrastructure
{
    public class CachedCustomerRepository : CachedRepository<Customer, int>
    {
        public CachedCustomerRepository(
            ITypeAccessorCache typeAccessorCache,
            ICacheProvider cacheProvider,
            IDatabase database)
            : base(typeAccessorCache, cacheProvider, database)
        {
        }

        protected override void ConfigureCache(Action<ICacheContext> configurator)
        {
            CacheConfigurator = config => config.SetAbsoluteExpiration(TimeSpan.FromSeconds(300));
        }
    }
}