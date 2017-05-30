using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Tests.Common;
using System;

namespace Skeleton.Tests.Infrastructure
{
    public abstract class OrmTestBase
    {
        private readonly IBootstrapper _host = new Bootstrapper();
        private readonly Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> _databaseConfigurator =
            builder => builder.UsingConnectionString(AppConfiguration.ConnectionString).Build();

        protected OrmTestBase()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();
            SqlDbSeeder.SeedCustomers();

            _host.UseDatabase(_databaseConfigurator)
                 .UseOrm()
                 .UseAsyncOrm();
        }

        protected IDependencyResolver Resolver => _host as IDependencyResolver;
    }
}