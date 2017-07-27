using System;

namespace Skeleton.Core
{
    public static class TypeExtensions
    {
        public static bool IsPrimitiveExtended(this Type type)
        {
            type.ThrowIfNull(nameof(type));

            if (type.IsPrimitive)
                return true;

            return (type == typeof(string)) ||
                   (type == typeof(decimal)) ||
                   (type == typeof(DateTime)) ||
                   (type == typeof(DateTimeOffset)) ||
                   (type == typeof(TimeSpan)) ||
                   (type == typeof(Guid));
        }
    }
}