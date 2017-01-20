using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Tests.Common;
using Skeleton.Tests.Web.Mock;

namespace Skeleton.Web.Tests
{
    [TestClass]
    public class AsyncHttpClientCrudTests
    {
        public AsyncHttpClientCrudTests()
        {
            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public async Task GetAllAsync()
        {
            using (var client = new AsyncCustomersHttpClient())
            {
                var results = await client.GetAllAsync();

                Assert.IsNotNull(results);
                Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
            }
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_ById()
        {
            using (var client = new AsyncCustomersHttpClient())
            {
                var data = await client.GetAllAsync();
                var firstCustomer = data.FirstOrDefault();

                Assert.IsNotNull(firstCustomer);

                var result = await client.FirstOrDefaultAsync(firstCustomer.CustomerId);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(CustomerDto));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task FirstOrDefault_With_Wrong_Id()
        {
            using (var client = new AsyncCustomersHttpClient())
            {
                await client.FirstOrDefaultAsync(100000);
            }
        }

        [TestMethod]
        public async Task PageAsync()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            using (var client = new AsyncCustomersHttpClient())
            {
                for (var page = 1; page < numberOfPages; ++page)
                {
                    var response = await client.PageAsync(pageSize, page);
                    Assert.IsTrue(response.Results.Count() <= pageSize);
                }
            }
        }

        [TestMethod]
        public async Task UpdateAsync()
        {
            using (var client = new AsyncCustomersHttpClient())
            {
                var data = await client.GetAllAsync();
                var firstCustomer = data.FirstOrDefault();

                Assert.IsNotNull(firstCustomer);

                var customer = new CustomerDto
                {
                    CustomerId = firstCustomer.CustomerId,
                    Name = "CustomerUpdated" + firstCustomer.CustomerId,
                    CustomerCategoryId = firstCustomer.CustomerCategoryId
                };
                var result = await client.UpdateAsync(customer);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_Multiple()
        {
            using (var client = new AsyncCustomersHttpClient())
            {
                var customersPagedResults = await client.PageAsync(5, 1);
                var customers = customersPagedResults.Results;
                Assert.IsNotNull(customers);

                foreach (var customer in customers)
                    customer.Name = "CustomerUpdated" + customer.CustomerId;

                var result = await client.UpdateAsync(customers);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task AddAsync()
        {
            using (var client = new AsyncCustomersHttpClient())
            {
                var customer = new CustomerDto {Name = "Customer"};
                var result = await client.AddAsync(customer);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(CustomerDto));
            }
        }

        [TestMethod]
        public async Task AddAsync_Multiple()
        {
            using (var client = new AsyncCustomersHttpClient())
            {
                var customers = MemorySeeder.SeedCustomerDtos(5).ToList();
                var results = await client.AddAsync(customers);

                Assert.IsNotNull(results);
                Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
            }
        }

        [TestMethod]
        public async Task DeleteAsync()
        {
            using (var client = new AsyncCustomersHttpClient())
            {
                var data = await client.GetAllAsync();
                var firstCustomer = data.FirstOrDefault();
                Assert.IsNotNull(firstCustomer);
                var result = await client.DeleteAsync(firstCustomer.CustomerId);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task DeleteAsync_Multiple()
        {
            using (var client = new AsyncCustomersHttpClient())
            {
                var data = await client.PageAsync(5, 1);
                var customers = data.Results;
                Assert.IsNotNull(customers);

                var result = await client.DeleteAsync(customers);
                Assert.IsTrue(result);
            }
        }
    }
}