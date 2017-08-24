using Microsoft.Practices.Unity;
using Skeleton.Abstraction.Dependency;
using Skeleton.Core;
using System;

namespace Skeleton.Infrastructure.Dependency
{
    public sealed class DependencyRegistrar : HideObjectMethodsBase, IDependencyRegistrar
    {
        private readonly IUnityContainer _unityContainer;

        public DependencyRegistrar(IUnityContainer unityContainer)
        {
            unityContainer.ThrowIfNull();

            _unityContainer = unityContainer;
        }

        public IDependencyRegistrar Instance<TType>(TType instance)
        {
            _unityContainer.RegisterInstance(instance);

            return this;
        }

        public IDependencyRegistrar Type(Type from, Type to)
        {
            return Type(from, to, DependencyLifetime.Transient);
        }

        public IDependencyRegistrar Type(Type from, Type to, DependencyLifetime lifetime)
        {
            var lifetimeManager = CreateLifetimeManager(lifetime);
            _unityContainer.RegisterType(from, to, lifetimeManager);

            return this;
        }

        public IDependencyRegistrar Type<TFrom, TTo>()
           where TTo : TFrom
        {
            return Type<TFrom, TTo>(DependencyLifetime.Transient);
        }

        public IDependencyRegistrar Type<TFrom, TTo>(DependencyLifetime lifetime)
            where TTo : TFrom
        {
            var lifetimeManager = CreateLifetimeManager(lifetime);
            _unityContainer.RegisterType<TFrom, TTo>(lifetimeManager);

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