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
            CacheConfigurator = config => config.SetAbsoluteExpiration(TimeSpan.FromSeconds(300));
        }
    }
}