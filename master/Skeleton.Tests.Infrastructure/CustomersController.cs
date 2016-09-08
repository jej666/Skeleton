using Skeleton.Abstraction.Repository;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomersController : CrudController<Customer, CustomerDto>
    {
        public CustomersController(ICrudRepository<Customer, CustomerDto> repository)
            : base(repository)
        {
        }
    }
}