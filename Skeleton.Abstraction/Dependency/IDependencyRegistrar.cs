using System;

namespace Skeleton.Abstraction.Dependency
{
    public interface IDependencyRegistrar : IHideObjectMethods
    {
        IDependencyRegistrar Instance<TType>(TType instance);

        IDependencyRegistrar Type(Type from, Type to, DependencyLifetime lifetime = DependencyLifetime.Transient);

        IDependencyRegistrar Type<TFrom, TTo>(DependencyLifetime lifetime = DependencyLifetime.Transient) where TTo : TFrom;

        bool IsRegistered(Type typeToCheck);

        bool IsRegistered<TType>();
    }
}