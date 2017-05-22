using System;
using System.Diagnostics.CodeAnalysis;

namespace Skeleton.Abstraction
{
    public interface IDependencyRegistrar : IHideObjectMethods
    {
        IDependencyRegistrar RegisterInstance<TType>(TType instance);

        IDependencyRegistrar RegisterType(Type from, Type to);

        IDependencyRegistrar RegisterType<TFrom, TTo>() where TTo : TFrom;
    }
}