using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Skeleton.Abstraction.Reflection
{
    public interface IFieldMetadata
    {
        IMemberAccessor GetField(string name);

        IMemberAccessor GetPrivateField(string name);

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<IMemberAccessor> GetAllFields();

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<IMemberAccessor> GetFields();

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<IMemberAccessor> GetDeclaredOnlyFields();
    }
}