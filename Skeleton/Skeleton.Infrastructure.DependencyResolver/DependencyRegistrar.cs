using System;
using Microsoft.Practices.Unity;
using Skeleton.Shared.Abstraction;
using Skeleton.Shared.CommonTypes;

namespace Skeleton.Infrastructure.DependencyResolver
{
    public sealed class DependencyRegistrar :
        HideObjectMethods,
        IDependencyRegistrar
    {
        private readonly IUnityContainer _unityContainer;

        public DependencyRegistrar(IUnityContainer container)
        {
            _unityContainer = container;
        }

        public IDependencyRegistrar RegisterInstance(Type serviceType, object instance)
        {
            _unityContainer.RegisterInstance(serviceType, instance);

            return this;
        }

        public IDependencyRegistrar RegisterInstance<TType>(TType instance)
        {
            _unityContainer.RegisterInstance(instance);

            return this;
        }

        public IDependencyRegistrar RegisterType(Type type)
        {
            _unityContainer.RegisterType(type);

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
    }
}