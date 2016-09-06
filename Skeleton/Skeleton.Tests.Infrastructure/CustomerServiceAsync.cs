using Skeleton.Abstraction;
using Skeleton.Common;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Service;

namespace Skeleton.Tests.Infrastructure
{
    public sealed class CustomerServiceAsync : ServiceAsync<Customer, int>
    {
        public CustomerServiceAsync(ILogger logger, IRepositoryAsync<Customer, int> repository)
            : base(logger, repository)
        {
        }
    }
}