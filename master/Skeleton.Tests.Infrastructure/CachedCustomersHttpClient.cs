using Skeleton.Web.Client;

namespace Skeleton.Tests.Infrastructure
{
    public class CachedCustomersHttpClient : CrudHttpClient<CustomerDto, int>
    {
        public CachedCustomersHttpClient()
            : base("http://localhost:8081/", "api/cachedcustomers")
        {
        }
    }
}