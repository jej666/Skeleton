using Skeleton.Abstraction.Startup;
using Skeleton.Infrastructure.DependencyInjection.Configuration;

namespace Skeleton.Infrastructure.DependencyInjection.Plugins
{
    public sealed class ConfigurationPlugin : IPlugin
    {
        public void Configure(IBootstrapper bootstrapper)
        {
            bootstrapper.RegisterType<IDatabaseConfigurationBuilder, DatabaseConfigurationBuilder>();
        }
    }
}