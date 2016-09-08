using Skeleton.Abstraction.Repository;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Infrastructure
{
    public class AsyncCustomersController : AsyncCrudController<Customer, CustomerDto>
    {
        public AsyncCustomersController(IAsyncCrudRepository<Customer, CustomerDto> repository)
            : base(repository)
        {
        }
    }
}