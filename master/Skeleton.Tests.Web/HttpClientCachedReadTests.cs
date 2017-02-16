using System.Linq;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Tests.Common;
using Skeleton.Tests.Web.Mock;

namespace Skeleton.Tests.Web
{
    [TestClass]
    public class HttpClientCachedReadTests: HttpTestBase
    {
        [TestMethod]
        public void GetAll()
        {
            using (var client = new CachedCustomersHttpClient())
            {
                var results = client.GetAll();

                Assert.IsNotNull(results);
                Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
            }
        }

        [TestMethod]
        public void FirstOrDefault_ById()
        {
            using (var client = new CachedCustomersHttpClient())
            {
                var data = client.Page(1, 1).Results.FirstOrDefault();

                Assert.IsNotNull(data);

                var result = client.FirstOrDefault(data.CustomerId);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(CustomerDto));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void FirstOrDefault_With_Wrong_Id()
        {
            using (var client = new CachedCustomersHttpClient())
            {
                client.FirstOrDefault(1000000);
            }
        }

        [TestMethod]
        public void Page()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            using (var client = new CachedCustomersHttpClient())
            {
                for (var page = 1; page < numberOfPages; ++page)
                {
                    var response = client.Page(pageSize, page);
                    Assert.IsTrue(response.Results.Count() <= pageSize);
                }
            }
        }
    }
}