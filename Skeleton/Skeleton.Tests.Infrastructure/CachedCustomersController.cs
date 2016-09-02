using Skeleton.Core.Repository;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Infrastructure
{
    public class CachedCustomersController : CachedReadController<Customer, int, CustomerDto>
    {
        public CachedCustomersController(
            ICachedReadRepository<Customer, int, CustomerDto> repository)
            : base(repository)
        {
        }
    }
}