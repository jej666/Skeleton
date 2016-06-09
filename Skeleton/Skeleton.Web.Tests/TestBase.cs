using Skeleton.Common;
using Skeleton.Infrastructure.Data.Configuration;
using Skeleton.Infrastructure.DependencyResolver;
using Skeleton.Tests.Infrastructure;
using Skeleton.Web.Owin;
using System;

namespace Skeleton.Web.Tests
{
    public abstract class TestBase
    {
        private static readonly Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> Configurator =
            builder => builder
                .UsingConfigConnectionString("Default")
                .UsingAdvancedSettings()
                .SetCommandTimeout(30)
                .SetRetryPolicyCount(3)
                .SetRetryPolicyInterval(1);

        protected TestBase()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();
            Bootstrapper.Initialize();
            Bootstrapper.UseDatabase(Configurator);

            Startup.StartServer();
        }

        protected static IDependencyResolver Container
        {
            get { return Bootstrapper.Resolver; }
        }
    }
}
