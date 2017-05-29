using Microsoft.Practices.Unity;
using Skeleton.Abstraction;
using Skeleton.Common;
using Skeleton.Infrastructure.DependencyInjection.LoggerExtension;
using System;
using System.Collections.Generic;

namespace Skeleton.Infrastructure.DependencyInjection
{
    public class AppHost : HideObjectMethodsBase, IAppHost
    {
        private readonly IUnityContainer _unityContainer;
        private readonly List<IPlugin> _plugins = new List<IPlugin>();

        public AppHost(): this (new UnityContainer())
        { 
        }

        public AppHost(IUnityContainer unityContainer)
        {
            unityContainer.ThrowIfNull(nameof(unityContainer));

            _unityContainer = unityContainer;
            _unityContainer.AddExtension(new LoggerConstructorInjectionExtension());

            AddPlugin(new CorePlugin());      
        }

        public IEnumerable<IPlugin> Plugins => _plugins;

        public IDependencyRegistrar RegisterInstance<TType>(TType instance)
        {
            _unityContainer.RegisterInstance(instance);

            return this;
        }

        public IDependencyRegistrar RegisterType(Type from, Type to)
        {
            _unityContainer.RegisterType(from, to);

            return this;
        }

        public IDependencyRegistrar RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            _unityContainer.RegisterType<TFrom, TTo>();

            return this;
        }

        public TService Resolve<TService>() where TService : class
        {
            return _unityContainer.Resolve<TService>();
        }

        public void AddPlugin(IPlugin plugin)
        {
            plugin.ThrowIfNull(nameof(plugin));

            plugin.Configure(this);
            _plugins.Add(plugin);
        }

        public void AddPlugins(IEnumerable<IPlugin> plugins)
        {
            plugins.ThrowIfNull(nameof(plugins));

            foreach (var plugin in plugins)
                AddPlugin(plugin);
        }
    }
}
