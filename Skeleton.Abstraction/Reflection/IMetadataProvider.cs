using System;
using System.Collections.Generic;

namespace Skeleton.Abstraction.Reflection
{
    public interface IMetadataProvider : IHideObjectMethods
    {
        IEnumerable<Type> Types { get; }

        IMetadata GetMetadata(Type type);

        IMetadata GetMetadata<TType>();

        void RemoveMetadata(Type type);
    }
}