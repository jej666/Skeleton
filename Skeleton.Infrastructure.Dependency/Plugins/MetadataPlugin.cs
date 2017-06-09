using Skeleton.Abstraction.Reflection;
using Skeleton.Abstraction.Dependency;
using Skeleton.Core.Reflection;

namespace Skeleton.Infrastructure.Dependency.Plugins
{
    public sealed class MetadataPlugin : IPlugin
    {
        public void Configure(IDependencyContainer container)
        {
            container.Register.Type<IMetadataProvider, MetadataProvider>();
        }
    }
}