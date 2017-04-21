using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Tests.Common;
using Skeleton.Web.Server;
using System;

namespace Skeleton.Tests.Web.Mock
{
    public sealed class OwinServer : IDisposable
    {
        private IDisposable _server;
        
        public void Dispose()
        {
            _server.Dispose();
        }

        public void Start(Uri baseUrl)
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();
            SqlDbSeeder.SeedCustomers();

            Bootstrapper.UseDatabase(
                builder => builder.UsingConfigConnectionString("Default")
                .Build());

            _server = Startup.StartServer(baseUrl);
        } 
    }
}
