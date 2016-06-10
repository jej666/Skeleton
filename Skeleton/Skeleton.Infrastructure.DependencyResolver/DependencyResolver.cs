using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Skeleton.Common;
using Skeleton.Abstraction;

namespace Skeleton.Infrastructure.DependencyResolver
{
    public sealed class DependencyResolver :
        HideObjectMethods,
        IDependencyResolver
    {
        private readonly IUnityContainer _unityContainer;

        public DependencyResolver(IUnityContainer container)
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