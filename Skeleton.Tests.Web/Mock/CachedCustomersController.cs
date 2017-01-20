using Skeleton.Abstraction.Repository;
using Skeleton.Tests.Common;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Web.Mock
{
    public class CachedCustomersController : CachedReadController<Customer, CustomerDto>
    {
        public CachedCustomersController(
            ICachedReadRepository<Customer, CustomerDto> repository)
            : base(repository)
        {
        }
    }
}