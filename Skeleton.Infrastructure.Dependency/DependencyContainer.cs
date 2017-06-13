using Microsoft.Practices.Unity;
using Skeleton.Abstraction.Dependency;
using Skeleton.Common;
using System;
using System.Collections.Generic;

namespace Skeleton.Infrastructure.Dependency
{
    public sealed class DependencyContainer : HideObjectMethodsBase, IDependencyContainer
    {
        private readonly IUnityContainer _unityContainer;
        private readonly IDependencyRegistrar _registrar;
        private readonly List<IPlugin> _plugins = new List<IPlugin>();
        private readonly static Lazy<DependencyContainer> ContainerInstance =
            new Lazy<DependencyContainer>(() => new DependencyContainer());
        
        public DependencyContainer() : this(new UnityContainer())
        {
        }

        public DependencyContainer(IUnityContainer unityContainer)
        {
            unityContainer.ThrowIfNull(nameof(unityContainer));

            _unityContainer = unityContainer;
            _registrar = new DependencyRegistrar(unityContainer);
        }

        public IUnityContainer UnityContainer => _unityContainer;

        public IDependencyRegistrar Register => _registrar;

        public TService Resolve<TService>() where TService : class
        {
            return _unityContainer.Resolve<TService>();
        }

        public static DependencyContainer Instance => ContainerInstance.Value;

        public IEnumerable<IPlugin> Plugins => _plugins;

        public IDependencyContainer AddPlugin(IPlugin plugin)
        {
            plugin.ThrowIfNull(nameof(plugin));

            plugin.Configure(this);
            _plugins.Add(plugin);

            return this;
        }

        public IDependencyContainer AddPlugins(IEnumerable<IPlugin> plugins)
        {
            plugins.ThrowIfNull(nameof(plugins));

            foreach (var plugin in plugins)
                AddPlugin(plugin);

            return this;
        }
    }
}