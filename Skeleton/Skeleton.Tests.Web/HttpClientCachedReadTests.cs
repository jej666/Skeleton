using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Web.Tests
{
    [TestClass]
    public class HttpClientCachedReadTests
    {
        public HttpClientCachedReadTests()
        {
            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public void GetAll()
        {
            using (var client = new CachedCustomersHttpClient())
            {
                var results = client.Get();

                Assert.IsNotNull(results);
                Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
            }
        }
    }
}