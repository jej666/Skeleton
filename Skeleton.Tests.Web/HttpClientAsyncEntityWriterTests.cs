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
    public class HttpClientAsyncEntityWriterTests
    {
        private readonly RestClient _client = new RestClient(AppConfiguration.AsyncCustomersUri);

        [Test]
        public async Task EntityWriter_UpdateAsync()
        {
            var customer = GetPaginatedCustomers(1, 1).FirstOrDefault();
            Assert.IsNotNull(customer);

            var updatedCustomer = new CustomerDto
            {
                CustomerId = customer.CustomerId,
                Name = "CustomerUpdated" + customer.CustomerId,
                CustomerCategoryId = customer.CustomerCategoryId
            };

            var response = await _client.PutAsync(
                request => request.AddResource("update")
                                  .WithBody(updatedCustomer));

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task EntityWriter_UpdateAsync_With_Wrong_Id()
        {
            var customer = new CustomerDto
            {
                CustomerId = 100000,
                Name = "CustomerUpdated"
            };

            var result = await _client.PutAsync(
                request => request.AddResource("update")
                                  .WithBody(customer));
            Assert.IsTrue(result.StatusCode == HttpStatusCode.NotFound);
        }

        [Test]
        public async Task EntityWriter_BatchUpdateAsync()
        {
            var customers = GetPaginatedCustomers(5, 1);
            Assert.IsNotNull(customers);

            foreach (var customer in customers)
                customer.Name = "CustomerUpdated" + customer.CustomerId;

            var result = await _client.PostAsync(
                request => request.AddResource("batchupdate").WithBody(customers));
            Assert.IsTrue(result.IsSuccessStatusCode);
        }

        [Test]
        public async Task EntityWriter_CreateAsync()
        {
            var customer = MemorySeeder.SeedCustomerDto();
            var result = await _client.PostAsync<CustomerDto>(
                request => request.AddResource("create").WithBody(customer));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(CustomerDto), result);
        }

        [Test]
        public async Task EntityWriter_CreateAsync_With_Wrong_Id()
        {
            var customer = GetPaginatedCustomers(1, 1).FirstOrDefault();
            Assert.IsNotNull(customer);

            var result = await _client.PostAsync(
                request => request.AddResource("create").WithBody(customer));
            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task EntityWriter_BatchCreateAsync()
        {
            var customers = MemorySeeder.SeedCustomerDtos(5).ToList();
            var response = await _client.PostAsync(
                request => request.AddResource("batchcreate").WithBody(customers));
            var results = response.AsEnumerable<CustomerDto>();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(CustomerDto), results.First());
        }

        [Test]
        public async Task EntityWriter_DeleteAsync()
        {
            var data = GetPaginatedCustomers(1, 1).FirstOrDefault();
            Assert.IsNotNull(data);

            var response = await _client.DeleteAsync(
                request => request.AddResource("delete")
                                  .AddResource(data.CustomerId.ToString()));

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task EntityWriter_BatchDelete()
        {
            var customers = GetPaginatedCustomers(5, 1);
            Assert.IsNotNull(customers);

            var result = await _client.PostAsync(
                request => request.AddResource("batchdelete")
                                  .WithBody(customers));
            Assert.IsTrue(result.IsSuccessStatusCode);
        }

        [Test]
        public async Task EntityWriter_Delete_With_Wrong_Id()
        {
            var result = await _client.DeleteAsync(request => request.AddResource("delete/100000"));
            Assert.IsTrue(result.StatusCode == HttpStatusCode.NotFound);
        }

        private IEnumerable<CustomerDto> GetPaginatedCustomers(int pageSize, int pageNumber)
        {
            return _client.GetAsync<QueryResult<CustomerDto>>(
                    request => request.AddResource("query")
                                      .AddQueryParameters(new Query { PageSize = pageSize, PageNumber = pageNumber }))
                    .Result.Items;
        }
    }
}