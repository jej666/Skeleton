using Skeleton.Abstraction;
using Skeleton.Abstraction.Orm;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Common
{
    public class AsyncCachedCustomersController :
        AsyncCachedReadController<Customer, CustomerDto>
    {
        public AsyncCachedCustomersController(
            ILogger logger,
            IAsyncCachedEntityReader<Customer> reader,
            IEntityMapper<Customer, CustomerDto> mapper)
            : base(logger, reader, mapper)
        {
        }
    }
}