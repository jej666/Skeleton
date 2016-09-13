using System.Collections.Generic;

namespace Skeleton.Abstraction
{
    public interface IFieldMetadata
    {
        int FieldsCount { get; }
        
        IMemberAccessor GetField(string name);

        IMemberAccessor GetPrivateField(string name);

        IEnumerable<IMemberAccessor> GetAllFields();

        IEnumerable<IMemberAccessor> GetFields();

        IEnumerable<IMemberAccessor> GetDeclaredOnlyFields();
    }
}