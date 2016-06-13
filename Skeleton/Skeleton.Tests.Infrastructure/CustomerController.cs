using Skeleton.Abstraction;
using Skeleton.Core.Service;
using Skeleton.Web.Server;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomerController : ReadOnlyController<Customer,int, CustomerDto>
    {
        public CustomerController(
            IReadOnlyService<Customer, int> service,
            IEntityMapper<Customer, int> mapper)
            : base (service, mapper)
        {
        }    
    }
}
