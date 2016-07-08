using System;

namespace Skeleton.Shared.Abstraction.Reflection
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