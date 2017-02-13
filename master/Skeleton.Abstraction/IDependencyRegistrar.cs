using System;
using System.Diagnostics.CodeAnalysis;

namespace Skeleton.Abstraction
{
    public interface IDependencyRegistrar : IHideObjectMethods
    {
        IDependencyRegistrar RegisterInstance(Type serviceType, object instance);

        IDependencyRegistrar RegisterInstance<TType>(TType instance);

        IDependencyRegistrar RegisterType(Type type);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "To")]
        IDependencyRegistrar RegisterType(Type from, Type to);

        [SuppressMessage("Microsoft.Design", "CA1004: GenericMethodsShouldProvideTypeParameter")]
        IDependencyRegistrar RegisterType<TFrom, TTo>() where TTo : TFrom;
    }
}