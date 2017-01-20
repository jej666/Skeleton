using Skeleton.Abstraction.Repository;
using Skeleton.Tests.Common;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Web.Mock
{
    public class AsyncCachedCustomersController : AsyncCachedReadController<Customer, CustomerDto>
    {
        public AsyncCachedCustomersController(IAsyncCachedReadRepository<Customer, CustomerDto> repository)
            : base(repository)
        {
        }
    }
}