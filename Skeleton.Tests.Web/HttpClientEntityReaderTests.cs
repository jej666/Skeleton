using NUnit.Framework;
using Skeleton.Tests.Common;
using Skeleton.Web.Client;
using System.Linq;

namespace Skeleton.Tests.Web
{
    [TestFixture]
    public class HttpClientEntityReaderTests
    {
        private const int PageSize = 50;
        private const int NumberOfPages = 5;

        private readonly JsonCrudHttpClient<CustomerDto> client =
            new JsonCrudHttpClient<CustomerDto>(AppConfiguration.CustomersUriBuilder);

        [Test]
        public void EntityReader_GetAll()
        {
            var results = client.GetAll();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(CustomerDto), results.First());
        }

        [Test]
        public void EntityReader_FirstOrDefault()
        {
            var data = client
                .Query(new Query { PageSize = 1, PageNumber = 1 })
                .Items.FirstOrDefault();

            Assert.IsNotNull(data);

            var result = client.FirstOrDefault(data.CustomerId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(CustomerDto), result);
        }

        [Test]
        public void EntityReader_FirstOrDefault_With_Wrong_Id()
        {
            Assert.Catch(typeof(HttpResponseMessageException), () => client.FirstOrDefault(100000));
        }

        [Test]
        public void EntityReader_Page()
        {
            for (var page = 1; page < NumberOfPages; ++page)
            {
                var response = client.Query(new Query
                {
                    PageSize = PageSize,
                    PageNumber = page
                });

                Assert.IsTrue(response.Items.Count() <= PageSize);
            }
        }

        [Test]
        public void EntityReader_Query()
        {
            var response = client.Query(new Query
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