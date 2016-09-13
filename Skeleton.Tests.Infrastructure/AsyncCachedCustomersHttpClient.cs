using Skeleton.Web.Client;

namespace Skeleton.Tests.Infrastructure
{
    public class AsyncCachedCustomersHttpClient : AsyncCrudHttpClient<CustomerDto>
    {
        public AsyncCachedCustomersHttpClient()
            : base("http://localhost:8081/", "api/asynccachedcustomers")
        {
        }
    }
}