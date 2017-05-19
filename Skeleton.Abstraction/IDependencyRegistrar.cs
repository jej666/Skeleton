using System;
using System.Diagnostics.CodeAnalysis;

namespace Skeleton.Abstraction
{
    public interface IDependencyRegistrar : IHideObjectMethods
    {
        IDependencyRegistrar RegisterInstance<TType>(TType instance);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "To")]
        IDependencyRegistrar RegisterType(Type from, Type to);

        [SuppressMessage("Microsoft.Design", "CA1004: GenericMethodsShouldProvideTypeParameter")]
        IDependencyRegistrar RegisterType<TFrom, TTo>() where TTo : TFrom;
    }
}