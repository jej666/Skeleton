using System.Collections.Generic;

namespace Skeleton.Abstraction.Reflection
{
    public interface IFieldMetadata
    {
        IMemberAccessor GetField(string name);

        IEnumerable<IMemberAccessor> GetFields();

        IEnumerable<IMemberAccessor> GetDeclaredOnlyFields();
    }
}