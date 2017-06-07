using Skeleton.Abstraction;
using Skeleton.Abstraction.Startup;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Tests.Common;
using System;

namespace Skeleton.Tests.Infrastructure
{
    public abstract class OrmTestBase
    {
        private readonly IBootstrapper _bootstrapper = new Bootstrapper();
        private readonly Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> _databaseConfigurator =
            builder => builder.UsingConnectionString(AppConfiguration.ConnectionString).Build();

        protected OrmTestBase()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();
            SqlDbSeeder.SeedCustomers();

            _bootstrapper.Builder.UseSqlServer(_databaseConfigurator).WithOrm();
        }

        protected IBootstrapper Bootstrapper => _bootstrapper;

        protected IDependencyResolver Resolver => _bootstrapper as IDependencyResolver;
    }
}