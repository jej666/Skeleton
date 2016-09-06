using System;

namespace Skeleton.Shared.Abstraction.Reflection
{
    public interface IMetadataProvider : IHideObjectMethods
    {
        IMetadata GetMetadata(Type type);

        IMetadata GetMetadata<TType>();

        void RemoveMetadata(Type type);
    }
}