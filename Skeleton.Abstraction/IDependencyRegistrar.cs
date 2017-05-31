using System;

namespace Skeleton.Abstraction
{
    public interface IDependencyRegistrar : IHideObjectMethods
    {
        IDependencyRegistrar RegisterInstance<TType>(TType instance);

        IDependencyRegistrar RegisterType(Type from, Type to, DependencyLifetime lifetime = DependencyLifetime.Transient);

        IDependencyRegistrar RegisterType<TFrom, TTo>(DependencyLifetime lifetime = DependencyLifetime.Transient) where TTo : TFrom;

        bool IsRegistered(Type typeToCheck);

        bool IsRegistered<TType>();
    }
}