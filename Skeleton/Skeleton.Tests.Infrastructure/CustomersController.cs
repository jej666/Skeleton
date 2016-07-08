using Skeleton.Abstraction;
using Skeleton.Core.Service;
using Skeleton.Web.Server;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomersController : CrudController<Customer, int, CustomerDto>
    {
        public CustomersController(
            ICrudService<Customer, int> service,
            IEntityMapper<Customer, int> mapper)
            : base(service, mapper)
        {
        }
    }
}