using System;

namespace Skeleton.Common.Extensions
{
    public static class ReflectionExtensions
    {
        public static Type GetNonNullableType(this Type type)
        {
            type.ThrowIfNull(() => type);

            return type.IsNullableType() ? type.GetGenericArguments()[0] : type;
        }

        public static bool IsNullableType(this Type type)
        {
            type.ThrowIfNull(() => type);

            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsNumeric(this Type type)
        {
            type = type.GetNonNullableType();
            if (type.IsEnum) return false;
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Decimal:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                    return true;
            }
            return false;
        }

        public static Type[] ToTypeArray(this object[] values)
        {
            if (values == null || values.Length == 0)
                return Type.EmptyTypes;

            var types = new Type[values.Length];

            for (var i = 0; i < types.Length; i++)
            {
                var obj = values[i];
                types[i] = obj != null ? obj.GetType() : null;
            }

            return types;
        }

        public static bool IsPrimitiveExtendedIncludingNullable(this Type type)
        {
            if (IsPrimitiveExtended(type))
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return IsPrimitiveExtended(type.GenericTypeArguments[0]);
            }

            return false;
        }

        public static bool IsPrimitiveExtended(this Type type)
        {
            if (type.IsPrimitive)
            {
                return true;
            }

            return type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid);
        }
    }
}