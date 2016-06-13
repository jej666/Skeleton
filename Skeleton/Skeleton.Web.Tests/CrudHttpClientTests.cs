using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction;
using Skeleton.Common;
using Skeleton.Infrastructure.Data.Configuration;
using Skeleton.Infrastructure.DependencyResolver;
using Skeleton.Tests.Infrastructure;
using Skeleton.Web.Client;
using Skeleton.Web.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.Web.Tests
{
    [TestClass]
    public class CrudHttpClientTests : TestBase
    {
        private static readonly Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> Configurator =
            builder => builder
                .UsingConfigConnectionString("Default")
                .UsingAdvancedSettings()
                .SetCommandTimeout(30)
                .SetRetryPolicyCount(3)
                .SetRetryPolicyInterval(1);

        private static IDisposable _owinServer;

        [AssemblyInitialize]
        public static void SetUp(TestContext context)
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();

            Bootstrapper.Initialize();
            Bootstrapper.UseDatabase(Configurator);
            Bootstrapper.Registrar.RegisterType(typeof(CustomerController));

            _owinServer = Startup.StartServer("http://localhost:8081/");
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            _owinServer.Dispose();
        }

        protected static IDependencyResolver Container
        {
            get { return Bootstrapper.Resolver; }
        }

        public CrudHttpClientTests()
        {
            SqlDbSeeder.SeedCustomers();
        }

        [TestMethod]
        public void GetAll()
        {
            using (var client = new CustomerHttpClient())
            {
                var results = client.GetAll();

                Assert.IsNotNull(results);
                Assert.IsInstanceOfType(results.First(), typeof(CustomerDto));
            }
        }
    }
}
