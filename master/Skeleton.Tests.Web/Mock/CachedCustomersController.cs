using Skeleton.Abstraction.Orm;
using Skeleton.Tests.Common;
using Skeleton.Web.Server.Controllers;

namespace Skeleton.Tests.Web.Mock
{
    public class CachedCustomersController : 
        CachedEntityReaderController<Customer, CustomerDto>
    {
        public CachedCustomersController(
            ICachedEntityReader<Customer> reader,
            IEntityMapper<Customer, CustomerDto> mapper)
            : base(reader, mapper)
        {
        }
    }
}