using Skeleton.Common;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Tests.Infrastructure
{
    public class CachedCustomerRepositoryAsync : CachedRepositoryAsyncBase<Customer, int>
    {
        public CachedCustomerRepositoryAsync(
            ITypeAccessorCache typeAccessorCache,
            ICacheProvider cacheProvider,
            IDatabaseAsync database)
            : base(typeAccessorCache, cacheProvider, database)
        {
        }
    }
}