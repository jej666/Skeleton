using System;
using System.Collections.Generic;

namespace Skeleton.Abstraction.Startup
{
    public interface IBootstrapper :
        IDependencyRegistrar,
        IDependencyResolver
    {
        IBootstrapperBuilder Builder { get; }

        IEnumerable<IPlugin> Plugins { get; }

        IBootstrapper AddPlugin(IPlugin plugin);

        IBootstrapper AddPlugins(IEnumerable<IPlugin> plugins);
    }
}