using Skeleton.Abstraction.Repository;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Infrastructure
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