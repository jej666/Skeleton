using System;

namespace Skeleton.Abstraction.Reflection
{
    public interface IMetadata :
        IInstanceMetadata,
        IFieldMetadata,
        IPropertyMetadata,
        IMethodMetadata,
        IHideObjectMethods
    {
        Type Type { get; }
    }
}