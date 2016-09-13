using Skeleton.Abstraction.Repository;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Infrastructure
{
    public class AsyncCachedCustomersController : AsyncCachedReadController<Customer, CustomerDto>
    {
        public AsyncCachedCustomersController(IAsyncCachedReadRepository<Customer, CustomerDto> repository)
            : base(repository)
        {
        }
    }
}