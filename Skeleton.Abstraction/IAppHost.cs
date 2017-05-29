using System.Collections.Generic;

namespace Skeleton.Abstraction
{
    public interface IAppHost: IDependencyRegistrar, IDependencyResolver
    {
        IEnumerable<IPlugin> Plugins { get; }

        void AddPlugin(IPlugin plugin);

        void AddPlugins(IEnumerable<IPlugin> plugins);
    }

    public interface IPlugin
    {
        void Configure(IAppHost host);
    }
}
