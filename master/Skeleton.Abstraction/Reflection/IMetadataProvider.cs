using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Skeleton.Abstraction.Reflection
{
    public interface IMetadataProvider : IHideObjectMethods
    {
        IEnumerable<Type> Types { get; }

        IMetadata GetMetadata(Type type);

        [SuppressMessage("Microsoft.Design", "CA1004: GenericMethodsShouldProvideTypeParameter")]
        IMetadata GetMetadata<TType>();

        void RemoveMetadata(Type type);
    }
}