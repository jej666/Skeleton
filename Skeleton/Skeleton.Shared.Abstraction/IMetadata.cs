using System;

namespace Skeleton.Core
{
    public interface IMetadata :
        IInstanceCreator,
        IFieldMetadata,
        IPropertyMetadata,
        IMethodMetadata,
        IHideObjectMethods
    {
        Type Type { get; }
    }
}