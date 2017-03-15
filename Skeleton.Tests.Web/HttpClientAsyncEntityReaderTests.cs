using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Tests.Common;
using Skeleton.Tests.Web.Mock;
using System.Diagnostics.CodeAnalysis;
using Skeleton.Web.Client;

namespace Skeleton.Tests.Web
{
    [TestClass]
    public class HttpClientAsyncEntityReaderTests
    {
        private readonly static AsyncCrudHttpClient<CustomerDto> Client =
             new AsyncCrudHttpClient<CustomerDto>(Constants.BaseAddress, Constants.AsyncCustomersUrl, 8081);

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [TestMethod]
        public async Task AsyncEntityReader_GetAllAsync()
        {
            var results = await Client.GetAllAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
        }

        [TestMethod]
        public async Task AsyncEntityReader_FirstOrDefaultAsync()
        {
            var data = await Client.PageAsync(1, 1);
            var firstCustomer = data.Results.FirstOrDefault();

            Assert.IsNotNull(firstCustomer);

            var result = await Client.FirstOrDefaultAsync(firstCustomer.CustomerId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CustomerDto));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task AsyncEntityReader_FirstOrDefault_With_Wrong_Id()
        {
            await Client.FirstOrDefaultAsync(100000);
        }

        [TestMethod]
        public async Task AsyncEntityReader_PageAsync()
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