using System;
using Skeleton.Common;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Tests.Infrastructure
{
    public class CachedCustomerRepositoryAsync : CachedRepositoryAsync<Customer, int>
    {
        public CachedCustomerRepositoryAsync(
            ITypeAccessorCache typeAccessorCache,
            ICacheProvider cacheProvider,
            IDatabaseAsync database)
            : base(typeAccessorCache, cacheProvider, database)
        {
        }

        protected override void ConfigureCache(Action<ICacheContext> configurator)
        {
            CacheConfigurator = config => config.SetAbsoluteExpiration(TimeSpan.FromSeconds(300));
        }
    }
}