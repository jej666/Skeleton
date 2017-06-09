using NUnit.Framework;
using Skeleton.Abstraction.Dependency;
using Skeleton.Tests.Common;
using System.Configuration;

namespace Skeleton.Tests.Infrastructure
{
    [TestFixture]
    public class DatabaseConfigurationTests : OrmTestBase
    {
        [Test]
        public void ConfigurationBuilder_DefaultSettings()
        {
            var defaultConfiguration = Resolver.Resolve<IDatabaseConfiguration>();
            var builder = Resolver.Resolve<IDatabaseConfigurationBuilder>();
            var configuration = builder.UsingConnectionString(AppConfiguration.ConnectionString)
                                      .Build();

            Assert.IsNotNull(configuration);
            Assert.IsTrue(configuration.ConnectionString.Equals(AppConfiguration.ConnectionString));
            Assert.IsTrue(configuration.Name.Equals(defaultConfiguration.Name));
            Assert.IsTrue(configuration.ProviderName.Equals(defaultConfiguration.ProviderName));
            Assert.IsTrue(configuration.RetryCount == defaultConfiguration.RetryCount);
            Assert.IsTrue(configuration.RetryInterval == defaultConfiguration.RetryInterval);
            Assert.IsTrue(configuration.CommandTimeout == defaultConfiguration.CommandTimeout);
        }

        [Test]
        public void ConfigurationBuilder_DefaultConfig_InAppSettings()
        {
            // Find a way AppVeyor can fetch info from connectionStrings in app.config
            if (AppConfiguration.AppVeyorBuild)
                return;

            var builder = Resolver.Resolve<IDatabaseConfigurationBuilder>();
            var configuration = builder.UsingConfigConnectionString("Default")
                                       .Build();

            Assert.IsNotNull(configuration);
            Assert.IsTrue(configuration.ConnectionString.Equals(AppConfiguration.ConnectionString));
            Assert.IsTrue(configuration.Name.Equals("Default"));
        }

        [Test]
        public void ConfigurationBuilder_WrongAppSettings()
        {
            var builder = Resolver.Resolve<IDatabaseConfigurationBuilder>();

            Assert.IsNotNull(builder);
            Assert.Catch(typeof(ConfigurationErrorsException), () => builder.UsingConfigConnectionString("Defaul"));
        }

        [Test]
        public void ConfigurationBuilder_DefaultConnectionString()
        {
            // Find a way AppVeyor can fetch info from connectionStrings in app.config
            if (AppConfiguration.AppVeyorBuild)
                return;

            var builder = Resolver.Resolve<IDatabaseConfigurationBuilder>();
            var configuration = builder.UsingDefaultConfigConnectionString()
                                       .Build();

            Assert.IsNotNull(configuration);
            Assert.IsTrue(configuration.ConnectionString.Equals(AppConfiguration.ConnectionString));
            Assert.IsTrue(configuration.Name.Equals("Default"));
        }

        [Test]
        public void ConfigurationBuilder_AdvancedSettings()
        {
            var builder = Resolver.Resolve<IDatabaseConfigurationBuilder>();
            var configuration = builder.UsingConnectionString(AppConfiguration.ConnectionString)
                                        .UsingAdvancedSettings()
                                        .SetCommandTimeout(200)
                                        .SetRetryPolicyCount(3)
                                        .SetRetryPolicyInterval(1);

            Assert.IsNotNull(configuration);
            Assert.IsTrue(configuration.ConnectionString.Equals(AppConfiguration.ConnectionString));
            Assert.IsTrue(configuration.Name == "Default");
            Assert.IsTrue(configuration.ProviderName == "System.Data.SqlClient");
            Assert.IsTrue(configuration.RetryCount == 3);
            Assert.IsTrue(configuration.RetryInterval == 1);
            Assert.IsTrue(configuration.CommandTimeout == 200);
        }
    }
}