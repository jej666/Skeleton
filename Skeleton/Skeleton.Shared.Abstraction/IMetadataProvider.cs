using System;

namespace Skeleton.Core
{
    public interface IMetadataProvider : IHideObjectMethods
    {
        IMetadata GetMetadata(Type type);

        IMetadata GetMetadata<TType>();

        void RemoveMetadata(Type type);
    }
}