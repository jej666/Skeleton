using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Tests.Common;
using Skeleton.Tests.Web.Mock;
using System.Linq;
using System.Net.Http;

namespace Skeleton.Tests.Web
{
    [TestClass]
    public class HttpClientCrudTests
    {
        private readonly static OwinServer Server = new OwinServer();
        private readonly static CustomersHttpClient Client = new CustomersHttpClient();

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Server.Start();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Server.Dispose();
        }

        [TestMethod]
        public void GetAll()
        {
            var results = Client.GetAll();

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
        }

        [TestMethod]
        public void FirstOrDefault_ById()
        {
            var data = Client.GetAll().FirstOrDefault();

            Assert.IsNotNull(data);

            var result = Client.FirstOrDefault(data.CustomerId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CustomerDto));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void FirstOrDefault_With_Wrong_Id()
        {
            Client.FirstOrDefault(100000);
        }

        [TestMethod]
        public void Page()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            for (var page = 1; page < numberOfPages; ++page)
            {
                var response = Client.Page(pageSize, page);
                Assert.IsTrue(response.Results.Count() <= pageSize);
            }
        }

        [TestMethod]
        public void Update()
        {
            var data = Client.GetAll().FirstOrDefault();

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

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void Update_With_Wrong_Id()
        {
            var customer = new CustomerDto
            {
                CustomerId = 100000,
                Name = "CustomerUpdated"
            };

            Client.Update(customer);
        }

        [TestMethod]
        public void Update_Multiple()
        {
            var customers = Client.Page(5, 1).Results.ToList();
            Assert.IsNotNull(customers);

            foreach (var customer in customers)
                customer.Name = "CustomerUpdated" + customer.CustomerId;

            var result = Client.Update(customers);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Add()
        {
            var customer = MemorySeeder.SeedCustomerDto();
            var result = Client.Add(customer);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CustomerDto));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void Add_With_Id()
        {
            var customer = MemorySeeder.SeedCustomerDto();
            customer.CustomerId = 100000;
            Client.Add(customer);
        }

        [TestMethod]
        public void Add_Multiple()
        {
            var customers = MemorySeeder.SeedCustomerDtos(5).ToList();
            var results = Client.Add(customers);

            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
        }

        [TestMethod]
        public void Delete()
        {
            var data = Client.Page(1, 1).Results.FirstOrDefault();
            var result = (data != null) && Client.Delete(data.CustomerId);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Delete_Multiple()
        {
            var customers = Client.Page(5, 1).Results;
            Assert.IsNotNull(customers);

            var result = Client.Delete(customers);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void Delete_With_Wrong_Id()
        {
            Client.Delete(100000);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void GetException()
        {
            var uri = Client.UriBuilder.StartNew().AppendAction("GetException").Uri;
            Client.Get(uri);
        }
    }
}