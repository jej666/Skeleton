using Skeleton.Abstraction.Dependency;
using Skeleton.Core;
using System.Collections.Generic;

namespace Skeleton.Infrastructure.Dependency.Plugins
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

        public void Configure(IDependencyContainer container)
        {
            container.ThrowIfNull();

            container.AddPlugins(_corePlugins);
        }
    }
}