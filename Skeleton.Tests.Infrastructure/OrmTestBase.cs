using Skeleton.Abstraction;
using Skeleton.Abstraction.Dependency;
using Skeleton.Infrastructure.Dependency;
using Skeleton.Tests.Common;
using System;

namespace Skeleton.Tests.Infrastructure
{
    public abstract class OrmTestBase
    {
        private readonly IBootstrapper _bootstrapper;
        private readonly Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> _databaseConfigurator =
            builder => builder.UsingConnectionString(AppConfiguration.ConnectionString).Build();

        protected OrmTestBase()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();
            SqlDbSeeder.SeedCustomers();

            _bootstrapper = new Bootstrapper();
            _bootstrapper.UseSqlServer(_databaseConfigurator).WithOrm();
        }

        protected IDependencyContainer Resolver => _bootstrapper.Container;
    }
}