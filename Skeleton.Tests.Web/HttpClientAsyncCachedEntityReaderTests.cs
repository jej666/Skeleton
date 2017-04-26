﻿using NUnit.Framework;
using Skeleton.Tests.Common;
using Skeleton.Web.Client;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.Tests.Web
{
    [TestFixture]
    public class AsyncCachedEntityReaderClientTests
    {
        private readonly static AsyncCrudHttpClient<CustomerDto> Client =
            new AsyncCrudHttpClient<CustomerDto>(AppConfiguration.AsyncCachedCustomersUriBuilder);

        [Test]
        public async Task AsyncCachedEntityReader_GetAllAsync()
        {
            var results = await Client.GetAllAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(CustomerDto), results.First());
        }

        [Test]
        public async Task AsyncCachedEntityReader_FirstOrDefaultAsync()
        {
            var data = await Client.PageAsync(1, 1);
            var first = data.Results.FirstOrDefault();

            Assert.IsNotNull(first);

            var result = await Client.FirstOrDefaultAsync(first.CustomerId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(CustomerDto), result);
        }

        [Test]
        public void AsyncCachedEntityReader_FirstOrDefaultAsync_With_Wrong_Id()
        {
            Assert.CatchAsync(typeof(CustomHttpException), async () => await Client.FirstOrDefaultAsync(100000));
        }

        [Test]
        public async Task AsyncCachedEntityReader_Page()
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