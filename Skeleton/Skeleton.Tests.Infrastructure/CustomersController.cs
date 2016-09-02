using Skeleton.Core.Repository;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomersController : CrudController<Customer, int, CustomerDto>
    {
        public CustomersController(ICrudService<Customer, int, CustomerDto> service)
            : base(service)
        {
        }
    }
}