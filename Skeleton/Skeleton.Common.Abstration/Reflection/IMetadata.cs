using System;

namespace Skeleton.Abstraction.Reflection
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