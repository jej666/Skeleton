using Skeleton.Abstraction.Repository;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomersController : CrudController<Customer, int, CustomerDto>
    {
        public CustomersController(ICrudRepository<Customer, int, CustomerDto> repository)
            : base(repository)
        {
        }
    }
}