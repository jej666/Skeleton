using NUnit.Framework;
using Skeleton.Tests.Common;
using Skeleton.Tests.Web.Mock;
using Skeleton.Web.Client;
using System.Linq;

namespace Skeleton.Tests.Web
{
    [TestFixture]
    public class HttpClientEntityReaderTests
    {
        private readonly static CrudHttpClient<CustomerDto> Client =
            new CrudHttpClient<CustomerDto>(AppConfiguration.CustomersUriBuilder);

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
        public void EntityReader_GetAll()
        {
            var results = Client.GetAll();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(CustomerDto), results.First());
        }

        [Test]
        public void EntityReader_FirstOrDefault()
        {
            var data = Client.Page(pageSize: 1, pageNumber: 1).Results.FirstOrDefault();

            Assert.IsNotNull(data);

            var result = Client.FirstOrDefault(data.CustomerId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(CustomerDto), result);
        }

        [Test]
        public void EntityReader_FirstOrDefault_With_Wrong_Id()
        {
            Assert.Catch(typeof(CustomHttpException), () => Client.FirstOrDefault(100000));
        }

        [Test]
        public void EntityReader_Page()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var response = Client.Page(pageSize, page);
                Assert.IsTrue(response.Results.Count() <= pageSize);
            }
        }

        //[Test]
        //public void EntityReader_GetException()
        //{
        //    var uri = Client.UriBuilder.StartNew().AppendAction("GetException").Uri;
        //    Assert.Catch(typeof(CustomHttpException), () => Client.Get(uri));
        //}
    }
}