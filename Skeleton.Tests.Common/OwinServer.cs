using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Web.Server;
using System;

namespace Skeleton.Tests.Common
{
    public sealed class OwinServer : IDisposable
    {
        private IDisposable _server;

        public OwinServer()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();

            if (!AppConfiguration.AppVeyorBuild)
                SqlDbSeeder.SeedCustomers();

            Bootstrapper.UseDatabase(
                builder => builder.UsingConnectionString(
                    AppConfiguration.ConnectionString)
                        .Build());
        }

        public void Start(Uri baseUrl)
        {
            _server = Startup.StartServer(baseUrl);
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}