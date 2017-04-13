using Skeleton.Abstraction;
using Skeleton.Abstraction.Orm;
using Skeleton.Tests.Common;
using Skeleton.Web.Server.Controllers;
using System.Web.Http;

namespace Skeleton.Tests.Web.Mock
{
    public class CustomersController : EntityCrudController<Customer, CustomerDto>
    {
        public CustomersController(
            ILogger logger,
            IEntityReader<Customer> reader,
            IEntityMapper<Customer, CustomerDto> mapper,
            IEntityWriter<Customer> writer)
            : base(logger, reader, mapper, writer)
        {
        }

        public IHttpActionResult GetException()
        {
            throw new System.Exception("OOps test!");
        }
    }
}