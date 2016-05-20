using Microsoft.Practices.Unity;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Data.Configuration;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class DataModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<IDatabaseConfigurationBuilder, DatabaseConfigurationBuilder>();
            var databaseConfiguration = Container.Resolve<IDatabaseConfigurationBuilder>()
                .UsingConfigConnectionString("Default")
                .UsingAdvancedSettings()
                .SetCommandTimeout(30)
                .SetRetryPolicyCount(3)
                .SetRetryPolicyInterval(1);
            Context.Container.RegisterInstance(databaseConfiguration);
            Container.RegisterType<IDatabaseFactory, DatabaseFactory>();
            Container.RegisterType<IDatabase, Database>();
            Container.RegisterType<IDatabaseAsync, DatabaseAsync>();
        }
    }
}