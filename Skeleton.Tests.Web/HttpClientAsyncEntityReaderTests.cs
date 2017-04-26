using NUnit.Framework;
using Skeleton.Tests.Common;
using Skeleton.Tests.Web.Mock;
using Skeleton.Web.Client;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.Tests.Web
{
    [TestFixture]
    public class HttpClientAsyncEntityReaderTests
    {
        private readonly static AsyncCrudHttpClient<CustomerDto> Client =
             new AsyncCrudHttpClient<CustomerDto>(AppConfiguration.AsyncCustomersUriBuilder);

        private readonly OwinServer _server = new OwinServer();

        [OneTimeSetUp]
        public void Init()
        {
            _server.Start(AppConfiguration.BaseUrl);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _server.Dispose();
        }

        [Test]
        public async Task AsyncEntityReader_GetAllAsync()
        {
            var results = await Client.GetAllAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(CustomerDto), results.First());
        }

        [Test]
        public async Task AsyncEntityReader_FirstOrDefaultAsync()
        {
            var data = await Client.PageAsync(1, 1);
            var firstCustomer = data.Results.FirstOrDefault();

            Assert.IsNotNull(firstCustomer);

            var result = await Client.FirstOrDefaultAsync(firstCustomer.CustomerId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(CustomerDto), result);
        }

        [Test]
        public void AsyncEntityReader_FirstOrDefault_With_Wrong_Id()
        {
            Assert.CatchAsync(typeof(CustomHttpException), async () => await Client.FirstOrDefaultAsync(100000));
        }

        [Test]
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