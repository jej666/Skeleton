using Skeleton.Abstraction;
using Skeleton.Core.Service;
using Skeleton.Web.Server;
using System.Web.Http;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomersController : Controller<Customer, int, CustomerDto>
    {
        public CustomersController(
            IService<Customer, int> service,
            IEntityMapper<Customer, int> mapper)
            : base(service, mapper)
        {
        }
    }
}
