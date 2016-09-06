using System;

namespace Skeleton.Abstraction
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