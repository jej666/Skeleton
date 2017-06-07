using Microsoft.Practices.Unity;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Startup;
using Skeleton.Common;
using Skeleton.Infrastructure.DependencyInjection.Configuration;
using Skeleton.Infrastructure.DependencyInjection.Plugins;
using System;
using System.Collections.Generic;

namespace Skeleton.Infrastructure.DependencyInjection
{
    public class Bootstrapper : HideObjectMethodsBase, IBootstrapper
    {
        private readonly IUnityContainer _unityContainer;
        private readonly List<IPlugin> _plugins = new List<IPlugin>();


        public Bootstrapper() : this(new UnityContainer())
        {
        }

        public Bootstrapper(IUnityContainer unityContainer)
        {
            unityContainer.ThrowIfNull(nameof(unityContainer));

            _unityContainer = unityContainer;
            _unityContainer.AddExtension(new LoggerConstructorInjectionExtension());

            AddPlugin(new CorePlugin());
        }

        public IEnumerable<IPlugin> Plugins => _plugins;

        public IBootstrapperBuilder Builder => new BootstrapperBuilder(this);
        
        public IDependencyRegistrar RegisterInstance<TType>(TType instance)
        {
            _unityContainer.RegisterInstance(instance);

            return this;
        }

        public IDependencyRegistrar RegisterType(Type from, Type to, DependencyLifetime lifetime = DependencyLifetime.Transient)
        {
            var lifetimeManager = CreateLifetimeManager(lifetime);
            _unityContainer.RegisterType(from, to, lifetimeManager);

            return this;
        }

        public IDependencyRegistrar RegisterType<TFrom, TTo>(DependencyLifetime lifetime = DependencyLifetime.Transient)
            where TTo : TFrom
        {
            var lifetimeManager = CreateLifetimeManager(lifetime);
            _unityContainer.RegisterType<TFrom, TTo>(lifetimeManager);

            return this;
        }

        public TService Resolve<TService>() where TService : class
        {
            return _unityContainer.Resolve<TService>();
        }

        public IBootstrapper AddPlugin(IPlugin plugin)
        {
            plugin.ThrowIfNull(nameof(plugin));

            plugin.Configure(this);
            _plugins.Add(plugin);

            return this;
        }

        public IBootstrapper AddPlugins(IEnumerable<IPlugin> plugins)
        {
            plugins.ThrowIfNull(nameof(plugins));

            foreach (var plugin in plugins)
                AddPlugin(plugin);

            return this;
        }

        public bool IsRegistered(Type typeToCheck)
        {
            return _unityContainer.IsRegistered(typeToCheck);
        }

        public bool IsRegistered<TType>()
        {
            return _unityContainer.IsRegistered<TType>();
        }

        private static LifetimeManager CreateLifetimeManager(DependencyLifetime lifetime)
        {
            switch (lifetime)
            {
                case DependencyLifetime.Scoped:
                    return new HierarchicalLifetimeManager();
                case DependencyLifetime.Singleton:
                    return new PerThreadLifetimeManager();
                default:
                    return new TransientLifetimeManager();
            }
        }
    }
}