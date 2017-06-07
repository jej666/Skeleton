using Skeleton.Abstraction.Startup;
using System.Collections.Generic;

namespace Skeleton.Infrastructure.DependencyInjection.Plugins
{
    public sealed class CorePlugin : IPlugin
    {
        private readonly List<IPlugin> _corePlugins = new List<IPlugin>
        {
            new LoggerPlugin(),
            new CachePlugin(),
            new MetadataPlugin(),
            new ConfigurationPlugin()
        };


        public void Configure(IBootstrapper bootstrapper)
        {
            bootstrapper.AddPlugins(_corePlugins);
        }
    }
}