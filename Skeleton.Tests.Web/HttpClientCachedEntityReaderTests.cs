using NUnit.Framework;
using Skeleton.Tests.Common;
using Skeleton.Web.Client;
using System.Linq;
using System.Net;
using System;

namespace Skeleton.Tests.Web
{
    [TestFixture]
    public class HttpClientCachedEntityReaderTests
    {
        private const int PageSize = 50;
        private const int NumberOfPages = 5;

        private readonly RestClient _client = new RestClient(new Uri(AppConfiguration.BaseAddress, "api/cachedcustomers"));

        [Test]
        public void EntityReader_GetAll()
        {
            var request = new RestRequest("getall");
            var results = _client.Get(request).AsEnumerable<CustomerDto>();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(CustomerDto), results.First());
        }

        [Test]
        public void EntityReader_FirstOrDefault()
        {
            var data = _client
                .Get<QueryResult<CustomerDto>>(
                    r => r.AddResource("query")
                          .AddQueryParameters(new Query { PageSize = 1, PageNumber = 1 }))
                .Items
                .FirstOrDefault();

            Assert.IsNotNull(data);

            var result = _client.Get<CustomerDto>(
                r => r.AddResource("firstordefault")
                      .AddResource(data.CustomerId.ToString()));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(CustomerDto), result);
        }

        [Test]
        public void EntityReader_FirstOrDefault_With_Wrong_Id()
        {
            var result = _client.Get(r => r.AddResource("firstordefault/100000"));
            Assert.IsTrue(result.StatusCode == HttpStatusCode.NotFound);
        }

        [Test]
        public void EntityReader_Page()
        {
            for (var page = 1; page < NumberOfPages; ++page)
            {
                var request = new RestRequest("query")
                    .AddQueryParameters(new Query { PageSize = PageSize, PageNumber = page });
                var result = _client.Get<QueryResult<CustomerDto>>(request);

                Assert.IsTrue(result.Items.Count() <= PageSize);
            }
        }

        [Test]
        public void EntityReader_Query()
        {
            var request = new RestRequest("query")
                    .AddQueryParameters(
                        new Query
                        {
                            Fields = "CustomerId,Name",
                            OrderBy = "CustomerId,-Name",
                            PageSize = 50,
                            PageNumber = 1
                        });

            var response = _client.Get(request);

            Assert.IsNotNull(response);
        }
    }
}