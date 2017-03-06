using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Tests.Common;
using Skeleton.Tests.Web.Mock;
using System.Diagnostics.CodeAnalysis;

namespace Skeleton.Tests.Web
{
    [TestClass]
    public class AsyncHttpClientCachedReadTests
    {
        private readonly static AsyncCachedCustomersHttpClient Client = new AsyncCachedCustomersHttpClient();

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [TestMethod]
        public async Task GetAllAsync()
        {
            var results = await Client.GetAllAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_ById()
        {
            var data = await Client.PageAsync(1, 1);
            var first = data.Results.FirstOrDefault();

            Assert.IsNotNull(first);

            var result = await Client.FirstOrDefaultAsync(first.CustomerId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CustomerDto));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task FirstOrDefaultAsync_With_Wrong_Id()
        {
            await Client.FirstOrDefaultAsync(100000);
        }

        [TestMethod]
        public async Task Page()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var response = await Client.PageAsync(pageSize, page);
                Assert.IsTrue(response.Results.Count() <= pageSize);
            }
        }
    }
}