using Skeleton.Abstraction;
using Skeleton.Abstraction.Orm;
using Skeleton.Web.Server.Controllers;
using System.Web.Http;

namespace Skeleton.Tests.Common
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

        [HttpGet]
        public IHttpActionResult Retry()
        {
            return InternalServerError();
        }
    }
}