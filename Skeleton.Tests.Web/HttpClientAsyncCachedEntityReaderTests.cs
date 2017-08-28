using NUnit.Framework;
using Skeleton.Tests.Common;
using Skeleton.Web.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Skeleton.Tests.Web
{
    [TestFixture]
    public class HttpClientAsyncCachedEntityReaderTests
    {
        private const int PageSize = 50;
        private const int NumberOfPages = 5;

        private readonly RestClient _client = new RestClient(new Uri(AppConfiguration.BaseAddress, "api/asynccachedcustomers"));

        [Test]
        public async Task CachedEntityReader_GetAllAsync()
        {
            var request = new RestRequest("all");
            var results = await _client.GetAsync<IEnumerable<CustomerDto>>(request);

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(CustomerDto), results.First());
        }

        [Test]
        public async Task CachedEntityReader_FirstOrDefaultAsync()
        {
            var data = await _client
                .GetAsync<QueryResult<CustomerDto>>(
                    r => r.AddResource("query")
                          .AddQueryParameters(new Query { PageSize = 1, PageNumber = 1 }));

            Assert.IsNotNull(data);
            var customer = data.Items.FirstOrDefault();
            var result = _client.Get<CustomerDto>(
                r => r.AddResource("firstordefault")
                      .AddResource(customer.CustomerId.ToString()));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(CustomerDto), result);
        }

        [Test]
        public async Task CachedEntityReader_FirstOrDefaultAsync_With_Wrong_Id()
        {
            var result = await _client.GetAsync(r => r.AddResource("firstordefault/100000"));
            Assert.IsTrue(result.StatusCode == HttpStatusCode.NotFound);
        }

        [Test]
        public async Task CachedEntityReader_PageAsync()
        {
            for (var page = 1; page < NumberOfPages; ++page)
            {
                var request = new RestRequest("query")
                    .AddQueryParameters(new Query { PageSize = PageSize, PageNumber = page });
                var result = await _client.GetAsync<QueryResult<CustomerDto>>(request);

                Assert.IsTrue(result.Items.Count() <= PageSize);
            }
        }

        [Test]
        public async Task CachedEntityReader_QueryAsync()
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

            var response = await _client.GetAsync(request);

            Assert.IsNotNull(response);
        }
    }
}