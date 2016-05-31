using System;
using Skeleton.Common;
using Skeleton.Infrastructure.Data.Configuration;
using Skeleton.Infrastructure.DependencyResolver;

namespace Skeleton.Tests
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
            Bootstrapper.Initialize();
            Bootstrapper.UseDatabase(Configurator);
        }

        protected static IDependencyResolver Container
        {
            get { return Bootstrapper.Resolver; }
        }
    }
}