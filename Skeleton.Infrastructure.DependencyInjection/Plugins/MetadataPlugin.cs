using Skeleton.Abstraction.Reflection;
using Skeleton.Abstraction.Startup;
using Skeleton.Core.Reflection;

namespace Skeleton.Infrastructure.DependencyInjection.Plugins
{
    public sealed class MetadataPlugin : IPlugin
    {
        public void Configure(IBootstrapper bootstrapper)
        {
            bootstrapper.RegisterType<IMetadataProvider, MetadataProvider>();
        }
    }
}