using System;
using Skeleton.Common;
using Skeleton.Infrastructure.Data.Configuration;
using Skeleton.Infrastructure.DependencyResolver;
using Skeleton.Tests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        [AssemblyInitialize]
        public static void SetUp(TestContext context)
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();
            Bootstrapper.Initialize();
            Bootstrapper.UseDatabase(Configurator);
        }

        protected static IDependencyResolver Container
        {
            get { return Bootstrapper.Resolver; }
        }
    }
}