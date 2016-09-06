using Skeleton.Common;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Service;

namespace Skeleton.Tests.Infrastructure
{
    public sealed class CachedCustomerServiceAsync : CachedServiceAsync<Customer, int>
    {
        public CachedCustomerServiceAsync(ILogger logger, ICachedRepositoryAsync<Customer, int> repository)
            : base(logger, repository)
        {
        }
    }
}