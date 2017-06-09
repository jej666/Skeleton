using Skeleton.Abstraction.Dependency;
using Skeleton.Infrastructure.Dependency.Configuration;

namespace Skeleton.Infrastructure.Dependency.Plugins
{
    public sealed class ConfigurationPlugin : IPlugin
    {
        public void Configure(IDependencyContainer container)
        {
            container.Register.Type<IDatabaseConfigurationBuilder, DatabaseConfigurationBuilder>();
        }
    }
}