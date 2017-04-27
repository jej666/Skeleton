using Skeleton.Abstraction;
using Skeleton.Abstraction.Orm;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Common
{
    public class AsyncCustomersController :
        AsyncEntityCrudController<Customer, CustomerDto>
    {
        public AsyncCustomersController(
            ILogger logger,
            IAsyncEntityReader<Customer> reader,
            IEntityMapper<Customer, CustomerDto> mapper,
            IAsyncEntityWriter<Customer> writer)
            : base(logger, reader, mapper, writer)
        {
        }
    }
}