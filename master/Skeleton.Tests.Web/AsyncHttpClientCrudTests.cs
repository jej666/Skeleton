using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Tests.Common;
using Skeleton.Tests.Web.Mock;
using System.Diagnostics.CodeAnalysis;

namespace Skeleton.Tests.Web
{
    [TestClass]
    public class AsyncHttpClientCrudTests
    {
        private readonly static AsyncCustomersHttpClient Client = new AsyncCustomersHttpClient();

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [TestMethod]
        public async Task GetAllAsync()
        {
            var results = await Client.GetAllAsync();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_ById()
        {
            var data = await Client.PageAsync(1, 1);
            var firstCustomer = data.Results.FirstOrDefault();

            Assert.IsNotNull(firstCustomer);

            var result = await Client.FirstOrDefaultAsync(firstCustomer.CustomerId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CustomerDto));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task FirstOrDefault_With_Wrong_Id()
        {
            await Client.FirstOrDefaultAsync(100000);
        }

        [TestMethod]
        public async Task PageAsync()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var response = await Client.PageAsync(pageSize, page);
                Assert.IsTrue(response.Results.Count() <= pageSize);
            }
        }

        [TestMethod]
        public async Task UpdateAsync()
        {
            var data = await Client.PageAsync(1, 1);
            var firstCustomer = data.Results.FirstOrDefault();

            Assert.IsNotNull(firstCustomer);

            var customer = new CustomerDto
            {
                CustomerId = firstCustomer.CustomerId,
                Name = "CustomerUpdated" + firstCustomer.CustomerId,
                CustomerCategoryId = firstCustomer.CustomerCategoryId
            };
            var result = await Client.UpdateAsync(customer);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task UpdateAsync_Multiple()
        {
            var customersPagedResults = await Client.PageAsync(5, 1);
            var customers = customersPagedResults.Results;

            Assert.IsNotNull(customers);

            foreach (var customer in customers)
                customer.Name = "CustomerUpdated" + customer.CustomerId;

            var result = await Client.UpdateAsync(customers);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task AddAsync()
        {
            var customer = new CustomerDto { Name = "Customer" };
            var result = await Client.AddAsync(customer);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CustomerDto));
        }

        [TestMethod]
        public async Task AddAsync_Multiple()
        {
            var customers = MemorySeeder.SeedCustomerDtos(5).ToList();
            var results = await Client.AddAsync(customers);

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
        }

        [TestMethod]
        public async Task DeleteAsync()
        {
            var data = await Client.PageAsync(1, 1);
            var firstCustomer = data.Results.FirstOrDefault();
            Assert.IsNotNull(firstCustomer);
            var result = await Client.DeleteAsync(firstCustomer.CustomerId);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteAsync_Multiple()
        {
            var data = await Client.PageAsync(5, 1);
            var customers = data.Results;
            Assert.IsNotNull(customers);

            var result = await Client.DeleteAsync(customers);
            Assert.IsTrue(result);
        }
    }
}