using Skeleton.Abstraction.Orm;
using Skeleton.Tests.Common;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Web.Mock
{
    public class AsyncCustomersController : 
        AsyncEntityCrudController<Customer, CustomerDto>
    {
        public AsyncCustomersController(
            IAsyncEntityReader<Customer> reader,
            IAsyncEntityWriter<Customer> writer,
            IEntityMapper<Customer, CustomerDto> mapper)
            : base(reader, writer, mapper)
        {
        }
    }
}