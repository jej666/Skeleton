using NUnit.Framework;
using Skeleton.Tests.Common;
using Skeleton.Web.Client;
using System.Linq;

namespace Skeleton.Tests.Web
{
    [TestFixture]
    public class HttpClientEntityReaderTests
    {
        private const int pageSize = 50;
        private const int numberOfPages = 5;

        private static readonly CrudHttpClient<CustomerDto> Client =
            new CrudHttpClient<CustomerDto>(AppConfiguration.CustomersUriBuilder);

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
            var data = Client
                .Query(new Query { PageSize = 1, PageNumber = 1 })
                .FirstOrDefault();

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
            for (var page = 1; page < numberOfPages; ++page)
            {
                var response = Client.Query(new Query
                {
                    PageSize = pageSize,
                    PageNumber = page
                });

                Assert.IsTrue(response.Count() <= pageSize);
            }
        }

        [Test]
        public void EntityReader_Query()
        {
            var response = Client.Query(new Query
            {
                Fields = "CustomerId,Name",
                OrderBy = "CustomerId,-Name",
                PageSize = 50,
                PageNumber = 1
            });

            Assert.IsNotNull(response);
        }
    }
}