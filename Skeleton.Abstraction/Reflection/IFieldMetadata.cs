using System.Collections.Generic;

namespace Skeleton.Abstraction.Reflection
{
    public interface IFieldMetadata
    {
        IMemberAccessor GetField(string name);

        IMemberAccessor GetPrivateField(string name);

        IEnumerable<IMemberAccessor> GetAllFields();

        IEnumerable<IMemberAccessor> GetFields();

        IEnumerable<IMemberAccessor> GetDeclaredOnlyFields();
    }
}