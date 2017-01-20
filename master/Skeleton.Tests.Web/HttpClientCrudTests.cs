using System;
using System.Linq;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Tests.Common;
using Skeleton.Web.Server;
using Skeleton.Tests.Web.Mock;

namespace Skeleton.Web.Tests
{
    [TestClass]
    public class HttpClientCrudTests
    {
        private static IDisposable _owinServer;

        public HttpClientCrudTests()
        {
            SqlDbSeeder.SeedCustomers();
        }

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();

            Bootstrapper.Initialize();
            Bootstrapper.UseDatabase(
                builder => builder.UsingConfigConnectionString("Default").Build());
            Bootstrapper.Registrar.RegisterType(typeof(CustomersController));

            _owinServer = Startup.StartServer("http://localhost:8081/");
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            _owinServer.Dispose();
        }

        [TestMethod]
        public void GetAll()
        {
            using (var client = new CustomersHttpClient())
            {
                var results = client.GetAll();

                Assert.IsNotNull(results);
                Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
            }
        }

        [TestMethod]
        public void FirstOrDefault_ById()
        {
            using (var client = new CustomersHttpClient())
            {
                var data = client.GetAll().FirstOrDefault();

                Assert.IsNotNull(data);

                var result = client.FirstOrDefault(data.CustomerId);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(CustomerDto));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void FirstOrDefault_With_Wrong_Id()
        {
            using (var client = new CustomersHttpClient())
            {
                client.FirstOrDefault(100000);
            }
        }

        [TestMethod]
        public void Page()
        {
            const int pageSize = 50;
            const int numberOfPages = 5;

            using (var client = new CustomersHttpClient())
            {
                for (var page = 1; page < numberOfPages; ++page)
                {
                    var response = client.Page(pageSize, page);
                    Assert.IsTrue(response.Results.Count() <= pageSize);
                }
            }
        }

        [TestMethod]
        public void Update()
        {
            using (var client = new CustomersHttpClient())
            {
                var data = client.GetAll().FirstOrDefault();

                Assert.IsNotNull(data);

                var customer = new CustomerDto
                {
                    CustomerId = data.CustomerId,
                    Name = "CustomerUpdated" + data.CustomerId,
                    CustomerCategoryId = data.CustomerCategoryId
                };
                var result = client.Update(customer);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void Update_With_Wrong_Id()
        {
            using (var client = new CustomersHttpClient())
            {
                var customer = new CustomerDto
                {
                    CustomerId = 100000,
                    Name = "CustomerUpdated"
                };

                client.Update(customer);
            }
        }

        [TestMethod]
        public void Update_Multiple()
        {
            using (var client = new CustomersHttpClient())
            {
                var customers = client.Page(5, 1).Results.ToList();
                Assert.IsNotNull(customers);

                foreach (var customer in customers)
                    customer.Name = "CustomerUpdated" + customer.CustomerId;

                var result = client.Update(customers);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void Add()
        {
            using (var client = new CustomersHttpClient())
            {
                var customer = MemorySeeder.SeedCustomerDto();
                var result = client.Add(customer);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(CustomerDto));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void Add_With_Id()
        {
            using (var client = new CustomersHttpClient())
            {
                var customer = MemorySeeder.SeedCustomerDto();
                customer.CustomerId = 100000;
                client.Add(customer);
            }
        }

        [TestMethod]
        public void Add_Multiple()
        {
            using (var client = new CustomersHttpClient())
            {
                var customers = MemorySeeder.SeedCustomerDtos(5).ToList();
                var results = client.Add(customers);

                Assert.IsNotNull(results);
                Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
            }
        }

        [TestMethod]
        public void Delete()
        {
            using (var client = new CustomersHttpClient())
            {
                var data = client.Page(1, 1).Results.FirstOrDefault();
                var result = (data != null) && client.Delete(data.CustomerId);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void Delete_Multiple()
        {
            using (var client = new CustomersHttpClient())
            {
                var customers = client.Page(5, 1).Results;
                Assert.IsNotNull(customers);

                var result = client.Delete(customers);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void Delete_With_Wrong_Id()
        {
            using (var client = new CustomersHttpClient())
            {
                client.Delete(100000);
            }
        }
    }
}