using Skeleton.Abstraction.Orm;
using Skeleton.Tests.Common;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Web.Mock
{
    public class AsyncCachedCustomersController : 
        AsyncCachedReadController<Customer, CustomerDto>
    {
        public AsyncCachedCustomersController(
            IAsyncCachedEntityReader<Customer> reader,
            IEntityMapper<Customer,CustomerDto> mapper)
            : base(reader, mapper)
        {
        }
    }
}