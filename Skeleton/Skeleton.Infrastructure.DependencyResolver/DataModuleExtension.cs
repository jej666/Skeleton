namespace Skeleton.Infrastructure.DependencyResolver
{
    using Data;
    using Data.Configuration;
    using Microsoft.Practices.Unity;

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
            Context.Container.RegisterInstance<IDatabaseConfiguration>(databaseConfiguration);
            Container.RegisterType<IDatabaseFactory, DatabaseFactory>();
            Container.RegisterType<IDatabase, Database>();
            Container.RegisterType<IDatabaseAsync, DatabaseAsync>();
        }
    }
}