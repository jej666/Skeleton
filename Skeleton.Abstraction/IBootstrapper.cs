using System.Collections.Generic;

namespace Skeleton.Abstraction
{
    public interface IBootstrapper :
        IDependencyRegistrar,
        IDependencyResolver
    {
        IEnumerable<IPlugin> Plugins { get; }

        void AddPlugin(IPlugin plugin);

        void AddPlugins(IEnumerable<IPlugin> plugins);
    }
}
