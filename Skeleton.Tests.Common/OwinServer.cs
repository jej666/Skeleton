using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Web.Server;
using System;

namespace Skeleton.Tests.Common
{
    public sealed class OwinServer : IDisposable
    {
        private IDisposable _server;
        private readonly Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> _databaseConfigurator =
            builder => builder.UsingConnectionString(AppConfiguration.ConnectionString).Build();

        public OwinServer()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();

            if (!AppConfiguration.AppVeyorBuild)
                SqlDbSeeder.SeedCustomers();

            OwinServerStartup.WebAppHost
                   .UseDatabase(_databaseConfigurator)
                   .UseOrm()
                   .UseAsyncOrm();
        }

        public void Start(Uri baseUrl)
        {
            _server = OwinServerStartup.StartServer(baseUrl);
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}