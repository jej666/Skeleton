using Skeleton.Core.Service;
using Skeleton.Web.Server;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Infrastructure
{
    public class CachedCustomersController : CachedReadController<Customer, int, CustomerDto>
    {
        public CachedCustomersController(
            ICachedReadService<Customer, int, CustomerDto> service)
            : base(service)
        {
        }
    }
}