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
        }

        public void Start(Uri baseUrl)
        {
            var startup = new OwinStartup();
            _server = startup.StartServer(baseUrl, 
                bootstrapper => bootstrapper
                    .UseSwagger()
                    .UseCheckModelForNull()
                    .UseValidateModelState()
                    .UseGlobalExceptionHandling()
                    .UseSqlServer(_databaseConfigurator)
                    .WithOrm());
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}