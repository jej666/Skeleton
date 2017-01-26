using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Tests.Common;
using Skeleton.Tests.Web.Mock;

namespace Skeleton.Tests.Web
{
    [TestClass]
    public class AsyncHttpClientCachedReadTests
    {
        public AsyncHttpClientCachedReadTests()
        {
            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public async Task GetAllAsync()
        {
            using (var client = new AsyncCachedCustomersHttpClient())
            {
                var results = await client.GetAllAsync();

                Assert.IsNotNull(results);
                Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
            }
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_ById()
        {
            using (var client = new AsyncCachedCustomersHttpClient())
            {
                var data = await client.PageAsync(1, 1);
                var first = data.Results.FirstOrDefault();

                Assert.IsNotNull(first);

                var result = await client.FirstOrDefaultAsync(first.CustomerId);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(CustomerDto));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task FirstOrDefaultAsync_With_Wrong_Id()
        {
            using (var client = new AsyncCachedCustomersHttpClient())
            {
                await client.FirstOrDefaultAsync(100000);
            }
        }

        [TestMethod]
        public async Task Page()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            using (var client = new AsyncCachedCustomersHttpClient())
            {
                for (var page = 1; page < numberOfPages; ++page)
                {
                    var response = await client.PageAsync(pageSize, page);
                    Assert.IsTrue(response.Results.Count() <= pageSize);
                }
            }
        }
    }
}