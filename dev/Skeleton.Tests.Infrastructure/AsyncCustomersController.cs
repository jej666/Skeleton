using Skeleton.Abstraction.Repository;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Infrastructure
{
    public class AsyncCustomersController : AsyncCrudController<Customer, int, CustomerDto>
    {
        public AsyncCustomersController(IAsyncCrudRepository<Customer, int, CustomerDto> repository)
            : base(repository)
        {
        }
    }
}