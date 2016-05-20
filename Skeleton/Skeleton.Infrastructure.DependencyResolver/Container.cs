using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Skeleton.Common;

namespace Skeleton.Infrastructure.DependencyResolver
{
    public class Container : HideObjectMethods, IContainer
    {
        private readonly IUnityContainer _unityContainer;

        public Container(IUnityContainer container)
        {
            _unityContainer = container;
        }

        public object Resolve(Type serviceType)
        {
            return _unityContainer.Resolve(serviceType);
        }

        public object Resolve(Type serviceType, string key)
        {
            return _unityContainer.Resolve(serviceType, key);
        }

        public TService Resolve<TService>() where TService : class
        {
            return _unityContainer.Resolve<TService>();
        }

        public TService Resolve<TService>(string key) where TService : class
        {
            return _unityContainer.Resolve<TService>(key);
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            return _unityContainer.ResolveAll(serviceType);
        }

        public IEnumerable<TService> ResolveAll<TService>() where TService : class
        {
            return _unityContainer.ResolveAll<TService>();
        }
    }
}