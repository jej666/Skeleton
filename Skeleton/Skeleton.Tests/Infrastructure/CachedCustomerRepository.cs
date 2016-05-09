using Skeleton.Common;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository;
using System;

namespace Skeleton.Tests.Infrastructure
{
    public class CachedCustomerRepository : CachedRepositoryBase<Customer, int>
    {
        public CachedCustomerRepository(
            ITypeAccessorCache typeAccessorCache,
            ICacheProvider cacheProvider,
            IDatabase database)
            : base(typeAccessorCache, cacheProvider, database)
        {
            CacheConfigurator = config => config.SetAbsoluteExpiration(TimeSpan.FromSeconds(300));
        }
    }
}