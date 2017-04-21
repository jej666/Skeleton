using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Tests.Common;
using Skeleton.Tests.Web.Mock;
using Skeleton.Web.Client;
using System.Linq;

namespace Skeleton.Tests.Web
{
    [TestClass]
    public class HttpClientEntityReaderTests
    {
        private readonly static OwinServer Server = new OwinServer();
        private readonly static CrudHttpClient<CustomerDto> Client =
            new CrudHttpClient<CustomerDto>(AppConfiguration.CustomersUriBuilder);

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Server.Start(AppConfiguration.BaseUrl);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Server.Dispose();
        }

        [TestMethod]
        public void EntityReader_GetAll()
        {
            var results = Client.GetAll();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
        }

        [TestMethod]
        public void EntityReader_FirstOrDefault()
        {
            var data = Client.Page(1, 1).Results.FirstOrDefault();

            Assert.IsNotNull(data);

            var result = Client.FirstOrDefault(data.CustomerId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CustomerDto));
        }

        [TestMethod]
        [ExpectedException(typeof(CustomHttpException))]
        public void EntityReader_FirstOrDefault_With_Wrong_Id()
        {
            Client.FirstOrDefault(100000);
        }

        [TestMethod]
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

        //[TestMethod]
        //[ExpectedException(typeof(CustomHttpException))]
        //public void EntityReader_GetException()
        //{
        //    var uri = Client.UriBuilder.StartNew().AppendAction("GetException").Uri;
        //    Client.Get(uri);
        //}
    }
}