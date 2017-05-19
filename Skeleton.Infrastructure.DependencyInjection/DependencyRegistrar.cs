using System;
using Microsoft.Practices.Unity;
using Skeleton.Abstraction;
using Skeleton.Common;

namespace Skeleton.Infrastructure.DependencyInjection
{
    public sealed class DependencyRegistrar :
        HideObjectMethodsBase,
        IDependencyRegistrar
    {
        private readonly IUnityContainer _unityContainer;

        public DependencyRegistrar(IUnityContainer container)
        {
            _unityContainer = container;
        }

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
    }
}