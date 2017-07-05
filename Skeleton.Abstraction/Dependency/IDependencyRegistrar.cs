using System;

namespace Skeleton.Abstraction.Dependency
{
    public interface IDependencyRegistrar : IHideObjectMethods
    {
        IDependencyRegistrar Instance<TType>(TType instance);

        IDependencyRegistrar Type(Type from, Type to);

        IDependencyRegistrar Type(Type from, Type to, DependencyLifetime lifetime);

        IDependencyRegistrar Type<TFrom, TTo>() where TTo : TFrom;

        IDependencyRegistrar Type<TFrom, TTo>(DependencyLifetime lifetime) where TTo : TFrom;

        bool IsRegistered(Type typeToCheck);

        bool IsRegistered<TType>();
    }
}