using Skeleton.Web.Client;

namespace Skeleton.Tests.Infrastructure
{
    public class CachedCustomersHttpClient : CrudHttpClient<CustomerDto>
    {
        public CachedCustomersHttpClient()
            : base("http://localhost:8081/", "api/cachedcustomers")
        {
        }
    }
}