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
    public abstract class HttpTestBase
    {
        private static IDisposable OwinServer;
        
        protected HttpTestBase()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();
            SqlDbSeeder.SeedCustomers();

            Bootstrapper.UseDatabase(
                builder => builder.UsingConfigConnectionString("Default")
                .Build());

            if (OwinServer == null)
                OwinServer = Startup.StartServer(Constants.BaseAddress);
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            OwinServer.Dispose();
        }
    }
}
