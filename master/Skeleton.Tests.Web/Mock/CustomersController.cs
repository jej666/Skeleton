using Skeleton.Abstraction.Repository;
using Skeleton.Tests.Common;
using Skeleton.Web.Server.Controllers;
using System.Web.Http;

namespace Skeleton.Tests.Web.Mock
{
    public class CustomersController : CrudController<Customer, CustomerDto>
    {
        public CustomersController(ICrudRepository<Customer, CustomerDto> repository)
            : base(repository)
        {
        }

        public IHttpActionResult GetException()
        {
            throw new System.Exception("OOps test!");
        }
    }
}