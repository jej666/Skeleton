using Skeleton.Core.Service;
using Skeleton.Web.Server;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomerController : ReadOnlyController<Customer,int>
    {
        public CustomerController(IReadOnlyService<Customer, int> service)
            : base (service)
        {
        }    
    }
}
