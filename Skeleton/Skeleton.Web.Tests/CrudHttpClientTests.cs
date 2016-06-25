using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Infrastructure.DependencyResolver;
using Skeleton.Tests.Infrastructure;
using Skeleton.Web.Server;
using System;
using System.Linq;

namespace Skeleton.Web.Tests
{
    [TestClass]
    public class CrudHttpClientTests
    {
        private static IDisposable _owinServer;

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

        public CrudHttpClientTests():base()
        {
            SqlDbSeeder.SeedCustomers();
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
                var result = client.Get(15);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(CustomerDto));
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
                    Assert.AreEqual(pageSize, response.Results.Count());
                }
            }
        }

        [TestMethod]
        public void Update()
        {
            using (var client = new CustomersHttpClient())
            {
                var customer = new CustomerDto { Name = "Customer" };
                var result = client.Put(15, customer);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void Add()
        {
            using (var client = new CustomersHttpClient())
            {
                var customer = new CustomerDto { Name = "Customer" };
                var result = client.Post(customer);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(CustomerDto));
            }
        }

        [TestMethod]
        public void Add_Multiple()
        {
            using (var client = new CustomersHttpClient())
            {
                var customers = MemorySeeder.SeedCustomerDtos(5).ToList();
                var results = client.Post(customers);

                Assert.IsNotNull(results);
                Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
            }
        }

        [TestMethod]
        public void Delete()
        {
            using (var client = new CustomersHttpClient())
            {
                var data = client.GetAll().FirstOrDefault();
                var result = client.Delete(data.CustomerId);

                Assert.IsTrue(result);
            }
        }
    }
}
