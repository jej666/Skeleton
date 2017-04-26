using NUnit.Framework;
using Skeleton.Tests.Common;
using Skeleton.Tests.Web.Mock;
using Skeleton.Web.Client;
using System.Linq;

namespace Skeleton.Tests.Web
{
    [TestFixture]
    public class HttpClientEntityWriterTests
    {
        private readonly static CrudHttpClient<CustomerDto> Client =
            new CrudHttpClient<CustomerDto>(AppConfiguration.CustomersUriBuilder);

        private readonly OwinServer _server = new OwinServer();

        [OneTimeSetUp]
        public void Init()
        {
            _server.Start(AppConfiguration.BaseUrl);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _server.Dispose();
        }

        [Test]
        public void EntityWriter_Update()
        {
            var data = Client.Page(1, 1).Results.FirstOrDefault();

            Assert.IsNotNull(data);

            var customer = new CustomerDto
            {
                CustomerId = data.CustomerId,
                Name = "CustomerUpdated" + data.CustomerId,
                CustomerCategoryId = data.CustomerCategoryId
            };
            var result = Client.Update(customer);

            Assert.IsTrue(result);
        }

        [Test]
        public void EntityWriter_Update_With_Wrong_Id()
        {
            var customer = new CustomerDto
            {
                CustomerId = 100000,
                Name = "CustomerUpdated"
            };

            Assert.Catch(typeof(CustomHttpException), () => Client.Update(customer));
        }

        [Test]
        public void EntityWriter_BatchUpdate()
        {
            var customers = Client.Page(5, 1).Results.ToList();
            Assert.IsNotNull(customers);

            foreach (var customer in customers)
                customer.Name = "CustomerUpdated" + customer.CustomerId;

            var result = Client.Update(customers);
            Assert.IsTrue(result);
        }

        [Test]
        public void EntityWriter_Create()
        {
            var customer = MemorySeeder.SeedCustomerDto();
            var result = Client.Create(customer);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(CustomerDto), result);
        }

        [Test]
        public void EntityWriter_Create_With_Wrong_Id()
        {
            var customer = MemorySeeder.SeedCustomerDto();
            customer.CustomerId = 100000;

            Assert.Catch(typeof(CustomHttpException), () => Client.Create(customer));
        }

        [Test]
        public void EntityWriter_BatchCreate()
        {
            var customers = MemorySeeder.SeedCustomerDtos(5).ToList();
            var results = Client.Create(customers);

            Assert.IsNotNull(results);
            Assert.IsInstanceOf(typeof(CustomerDto), results.First());
        }

        [Test]
        public void EntityWriter_Delete()
        {
            var data = Client.Page(1, 1).Results.FirstOrDefault();
            var result = (data != null) && Client.Delete(data.CustomerId);

            Assert.IsTrue(result);
        }

        [Test]
        public void EntityWriter_BatchDelete()
        {
            var customers = Client.Page(5, 1).Results;
            Assert.IsNotNull(customers);

            var result = Client.Delete(customers);
            Assert.IsTrue(result);
        }

        [Test]
        public void EntityWriter_Delete_With_Wrong_Id()
        {
            Assert.Catch(typeof(CustomHttpException), () => Client.Delete(100000));
        }
    }
}