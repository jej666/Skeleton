using Skeleton.Abstraction;
using Skeleton.Common;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Service;

namespace Skeleton.Tests.Infrastructure
{
    public sealed class CachedCustomerService : CachedService<Customer, int>
    {
        public CachedCustomerService(ILogger logger, ICachedRepository<Customer, int> repository)
            : base(logger, repository)
        {
        }
    }
}