using NUnit.Framework;
using Skeleton.Tests.Common;
using Skeleton.Web.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Skeleton.Tests.Web
{
    [TestFixture]
    public class HttpClientEntityWriterTests
    {
        private readonly RestClient _client = new RestClient(new Uri(AppConfiguration.BaseAddress, "api/customers"), new NoRetryPolicy());

        [Test]
        public void EntityWriter_Update()
        {
            var customer = GetPaginatedCustomers(1, 1).FirstOrDefault();
            Assert.IsNotNull(customer);

            var updatedCustomer = new CustomerDto
            {
                CustomerId = customer.CustomerId,
                Name = "CustomerUpdated" + customer.CustomerId,
                CustomerCategoryId = customer.CustomerCategoryId
            };

            var response = _client.Put(
                request => request.AddResource("update")
                                  .AddResource(updatedCustomer.CustomerId.ToString())
                                  .WithBody(updatedCustomer));

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public void EntityWriter_Update_With_Wrong_Id()
        {
            var customer = new CustomerDto
            {
                CustomerId = 100000,
                Name = "CustomerUpdated"
            };

            var result=  _client.Put(
                request => request.AddResource("update")
                                  .AddResource(customer.CustomerId.ToString())
                                  .WithBody(customer));
            Assert.IsTrue(result.StatusCode == HttpStatusCode.NotFound);
        }

        [Test]
        public void EntityWriter_BatchUpdate()
        {
            var customers = GetPaginatedCustomers(5, 1);
            Assert.IsNotNull(customers);

            foreach (var customer in customers)
                customer.Name = "CustomerUpdated" + customer.CustomerId;

            var result = _client.Post(
                request => request.AddResource("batchupdate").WithBody(customers));
            Assert.IsTrue(result.IsSuccessStatusCode);
        }

        [Test]
        public void EntityWriter_Create()
        {
            var customer = MemorySeeder.SeedCustomerDto();
            var result = _client.Post(
                    request => request.AddResource("create")
                                      .WithBody(customer))
                .As<CustomerDto>();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(CustomerDto), result);
        }

        [Test]
        public void EntityWriter_Create_With_Wrong_Id()
        {
            var customer = GetPaginatedCustomers(1, 1).FirstOrDefault();
            var result = _client.Post(
                request => request.AddResource("create").WithBody(customer));
            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test]
        public void EntityWriter_BatchCreate()
        {
            var customers = MemorySeeder.SeedCustomerDtos(5).ToList();
            var results = _client.Post(
                request => request.AddResource("batchcreate").WithBody(customers))
                                 .AsEnumerable<CustomerDto>();

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(CustomerDto), results.First());
        }

        [Test]
        public void EntityWriter_Delete()
        {
            var data = GetPaginatedCustomers(1, 1).FirstOrDefault();
            var result = (data != null) && _client.Delete(
                request => request.AddResource("delete")
                                  .AddResource(data.CustomerId.ToString()))
                    .IsSuccessStatusCode;

            Assert.IsTrue(result);
        }

        [Test]
        public void EntityWriter_BatchDelete()
        {
            var customers = GetPaginatedCustomers(5, 1);
            Assert.IsNotNull(customers);

            var result = _client.Post(
                request => request.AddResource("batchdelete")
                                  .WithBody(customers));
            Assert.IsTrue(result.IsSuccessStatusCode);
        }

        [Test]
        public void EntityWriter_Delete_With_Wrong_Id()
        {
            var result= _client.Delete(request => request.AddResource("delete/100000"));
            Assert.IsTrue(result.StatusCode == HttpStatusCode.NotFound);
        }

        private IEnumerable<CustomerDto> GetPaginatedCustomers(int pageSize, int pageNumber)
        {
            return _client.Get<QueryResult<CustomerDto>>(
                    request => request.AddResource("query")
                                      .AddQueryParameters(new Query
                                      {
                                          PageSize = pageSize,
                                          PageNumber = pageNumber
                                      }))
                          .Items;
        }
    }
}