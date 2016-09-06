using Skeleton.Web.Client;

namespace Skeleton.Tests.Infrastructure
{
    public class AsyncCustomersHttpClient : AsyncCrudHttpClient<CustomerDto, int>
    {
        public AsyncCustomersHttpClient()
            : base("http://localhost:8081/", "api/asynccustomers")
        {
        }
    }
}