using NUnit.Framework;
using Skeleton.Tests.Common;
using Skeleton.Web.Client;
using System.Linq;

namespace Skeleton.Tests.Web
{
    [TestFixture]
    public class HttpClientCachedEntityReaderTests
    {
        private readonly static CrudHttpClient<CustomerDto> Client =
            new CrudHttpClient<CustomerDto>(AppConfiguration.CachedCustomersUriBuilder);

        [Test]
        public void CachedEntityReader_GetAll()
        {
            var results = Client.GetAll();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(CustomerDto), results.First());
        }

        [Test]
        public void CachedEntityReader_FirstOrDefault()
        {
            var data = Client.Page(1, 1).Results.FirstOrDefault();

            Assert.IsNotNull(data);

            var result = Client.FirstOrDefault(data.CustomerId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(CustomerDto), result);
        }

        [Test]
        public void CachedEntityReader_FirstOrDefault_With_Wrong_Id()
        {
            Assert.Catch(typeof(CustomHttpException), () => Client.FirstOrDefault(1000000));
        }

        [Test]
        public void CachedEntityReader_Page()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var response = Client.Page(pageSize, page);
                Assert.IsTrue(response.Results.Count() <= pageSize);
            }
        }
    }
}