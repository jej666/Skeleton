using System;
using System.Collections.Generic;
using System.Reflection;

namespace Skeleton.Common
{
    public interface IDependencyRegistrar : IHideObjectMethods
    {
        IDependencyRegistrar RegisterInstance(Type serviceType, object instance);

        IDependencyRegistrar RegisterInstance<TType>(TType instance);

        IDependencyRegistrar RegisterType(Type from, Type to);

        IDependencyRegistrar RegisterType<TFrom, TTo>() where TTo : TFrom;

        void RegisterTypes(IEnumerable<Assembly> assembliesToLoad);
    }
}