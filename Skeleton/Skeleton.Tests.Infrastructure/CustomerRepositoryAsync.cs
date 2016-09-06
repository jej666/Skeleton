using Skeleton.Abstraction;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomerRepositoryAsync : RepositoryAsync<Customer, int>
    {
        public CustomerRepositoryAsync(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseAsync database)
            : base(typeAccessorCache, database)
        {
        }
    }
}