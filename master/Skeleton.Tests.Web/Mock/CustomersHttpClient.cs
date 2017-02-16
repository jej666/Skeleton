using Skeleton.Tests.Common;
using Skeleton.Web.Client;

namespace Skeleton.Tests.Web.Mock
{
    public class CustomersHttpClient : CrudHttpClient<CustomerDto>
    {
        public CustomersHttpClient()
            : base(Constants.BaseAddress, "api/customers")
        {
        }
    }
}