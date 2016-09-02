using Microsoft.Practices.Unity;
using Skeleton.Core.Data;
using Skeleton.Infrastructure.Data;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class DataModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<IDatabaseConfigurationBuilder, DatabaseConfigurationBuilder>();
            Container.RegisterType<IDatabaseFactory, DatabaseFactory>();
            Container.RegisterType<IDatabase, Database>();
            Container.RegisterType<IDatabaseAsync, DatabaseAsync>();
        }
    }
}