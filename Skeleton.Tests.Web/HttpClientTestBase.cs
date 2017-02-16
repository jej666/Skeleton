using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Tests.Common;
using Skeleton.Tests.Web.Mock;
using Skeleton.Web.Server;
using System;
using System.Net.Http;

namespace Skeleton.Tests.Web
{
    public abstract class HttpClientTestBase
    {
        private static IDisposable OwinServer;
        
        protected HttpClientTestBase()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();
            SqlDbSeeder.SeedCustomers();

            Bootstrapper.Initialize();
            Bootstrapper.UseDatabase(
                builder => builder.UsingConfigConnectionString("Default").Build());

            RegisterControllers();

            if (OwinServer == null)
                OwinServer = Startup.StartServer(Constants.BaseAddress);
        }

        private static void RegisterControllers()
        {
            Bootstrapper.Registrar
               .RegisterType(typeof(CustomersController))
               .RegisterType(typeof(CachedCustomersController))
               .RegisterType(typeof(AsyncCustomersController))
               .RegisterType(typeof(AsyncCachedCustomersController));
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            OwinServer.Dispose();
        }
    }
}
