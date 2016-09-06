using Skeleton.Abstraction;
using Skeleton.Common;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Service;

namespace Skeleton.Tests.Infrastructure
{
    public sealed class CustomerService : Service<Customer, int>
    {
        public CustomerService(ILogger logger, IRepository<Customer, int> repository)
            : base(logger, repository)
        {
        }
    }
}