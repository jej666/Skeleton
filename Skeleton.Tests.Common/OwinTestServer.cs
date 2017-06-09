using Skeleton.Web.Server;
using System;
using DatabaseConfigFunc = System.Func<
    Skeleton.Abstraction.Dependency.IDatabaseConfigurationBuilder,
    Skeleton.Abstraction.Dependency.IDatabaseConfiguration>;

namespace Skeleton.Tests.Common
{
    public sealed class OwinTestServer : IDisposable
    {
        private IDisposable _server;

        private readonly DatabaseConfigFunc _databaseConfigurator =
            builder => builder.UsingConnectionString(AppConfiguration.ConnectionString).Build();

        public OwinTestServer()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();

            if (!AppConfiguration.AppVeyorBuild)
                SqlDbSeeder.SeedCustomers();

            OwinStartup.Bootstrapper
                       .Builder
                       .UseSqlServer(_databaseConfigurator)
                       .WithOrm();
        }

        public void Start(Uri baseUrl)
        {
            _server = OwinStartup.StartServer(baseUrl);
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}