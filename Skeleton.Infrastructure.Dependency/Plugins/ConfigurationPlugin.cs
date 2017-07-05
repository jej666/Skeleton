using Skeleton.Abstraction.Dependency;
using Skeleton.Common;
using Skeleton.Infrastructure.Dependency.Configuration;

namespace Skeleton.Infrastructure.Dependency.Plugins
{
    public sealed class ConfigurationPlugin : IPlugin
    {
        public void Configure(IDependencyContainer container)
        {
            container.ThrowIfNull(nameof(container));

            container.Register.Type<IDatabaseConfigurationBuilder, DatabaseConfigurationBuilder>();
        }
    }
}