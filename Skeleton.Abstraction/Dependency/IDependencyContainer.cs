using System.Collections.Generic;

namespace Skeleton.Abstraction.Dependency
{
    public interface IDependencyContainer : IDependencyResolver, IHideObjectMethods
    {
        IDependencyRegistrar Register { get; }

        IEnumerable<IPlugin> Plugins { get; }

        IDependencyContainer AddPlugin(IPlugin plugin);

        IDependencyContainer AddPlugins(IEnumerable<IPlugin> plugins);
    }
}