using Skeleton.Web.Client;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomersHttpClient : CrudHttpClient<CustomerDto, int>
    {
        public CustomersHttpClient()
            : base("http://localhost:8081/", "api/customers")
        {
        }
    }
}
