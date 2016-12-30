using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Skeleton.Common
{
    public static class TypeExtensions
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
                   (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static bool IsNumeric(this Type type)
        {
            type.ThrowIfNull(() => type);

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

        public static Assembly GetAssembly(this Type type)
        {
            type.ThrowIfNull(() => type);

            return type.GetTypeInfo().Assembly;
        }

        public static bool IsArray(this Type type)
        {
            type.ThrowIfNull(() => type);

            return type.GetTypeInfo().BaseType == typeof(Array);
        }

        public static bool IsCollection(this Type type)
        {
            type.ThrowIfNull(() => type);

            var collectionType = typeof(ICollection<>);

            return type.GetTypeInfo().IsGenericType && type
                       .GetInterfaces()
                       .Any(i => i.GetTypeInfo().IsGenericType && (i.GetGenericTypeDefinition() == collectionType));
        }

        public static bool IsEnumerable(this Type type)
        {
            type.ThrowIfNull(() => type);

            var enumerableType = typeof(IEnumerable<>);

            return type.GetTypeInfo().IsGenericType && (type.GetGenericTypeDefinition() == enumerableType);
        }

        public static TypeCode GetTypeCode(this Type type)
        {
            type.ThrowIfNull(() => type);

            if (type == typeof(bool))
                return TypeCode.Boolean;
            if (type == typeof(char))
                return TypeCode.Char;
            if (type == typeof(sbyte))
                return TypeCode.SByte;
            if (type == typeof(byte))
                return TypeCode.Byte;
            if (type == typeof(short))
                return TypeCode.Int16;
            if (type == typeof(ushort))
                return TypeCode.UInt16;
            if (type == typeof(int))
                return TypeCode.Int32;
            if (type == typeof(uint))
                return TypeCode.UInt32;
            if (type == typeof(long))
                return TypeCode.Int64;
            if (type == typeof(ulong))
                return TypeCode.UInt64;
            if (type == typeof(float))
                return TypeCode.Single;
            if (type == typeof(double))
                return TypeCode.Double;
            if (type == typeof(decimal))
                return TypeCode.Decimal;
            if (type == typeof(DateTime))
                return TypeCode.DateTime;
            if (type == typeof(string))
                return TypeCode.String;
            if (type.GetTypeInfo().IsEnum)
                return GetTypeCode(Enum.GetUnderlyingType(type));
            return TypeCode.Object;
        }

        public static Type[] ToTypeArray(this object[] values)
        {
            if ((values == null) || (values.Length == 0))
                return Type.EmptyTypes;

            var types = new Type[values.Length];

            for (var i = 0; i < types.Length; i++)
            {
                var obj = values[i];
                types[i] = obj?.GetType();
            }

            return types;
        }

        public static bool IsPrimitiveExtendedIncludingNullable(this Type type)
        {
            type.ThrowIfNull(() => type);

            if (IsPrimitiveExtended(type))
                return true;

            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return IsPrimitiveExtended(type.GenericTypeArguments[0]);

            return false;
        }

        public static bool IsPrimitiveExtended(this Type type)
        {
            type.ThrowIfNull(() => type);

            if (type.IsPrimitive)
                return true;

            return (type == typeof(string)) ||
                   (type == typeof(decimal)) ||
                   (type == typeof(DateTime)) ||
                   (type == typeof(DateTimeOffset)) ||
                   (type == typeof(TimeSpan)) ||
                   (type == typeof(Guid));
        }

        public static bool IsInstantiatableType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.GetTypeInfo().IsAbstract || type.GetTypeInfo().IsInterface || type.IsArray)
                return false;

            if (type.GetTypeInfo().IsGenericType)
                return false;

            if (!HasDefaultConstructor(type))
                return false;

            return true;
        }

        public static bool HasDefaultConstructor(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var hasDefaultConstructor =
                type.GetTypeInfo().DeclaredConstructors.Any(ctor => !ctor.GetParameters().Any());

            return hasDefaultConstructor;
        }

        public static bool IsAssignable(this Type to, Type from)
        {
            to.ThrowIfNull(() => to);
            from.ThrowIfNull(() => from);

            if (to.IsAssignableFrom(from))
                return true;

            if (to.GetTypeInfo().IsGenericType && from.GetTypeInfo().IsGenericTypeDefinition)
                return to.IsAssignableFrom(from.MakeGenericType(to.GetGenericArguments()));

            return false;
        }

        public static bool IsSubClass(this Type type, Type check)
        {
            while (true)
            {
                if ((type == null) || (check == null))
                    return false;

                if (type == check)
                    return true;

                if (check.GetTypeInfo().IsInterface)
                    if (type.GetInterfaces().Any(t => IsSubClass(t, check)))
                        return true;

                if (type.GetTypeInfo().IsGenericType && !type.GetTypeInfo().IsGenericTypeDefinition)
                    if (IsSubClass(type.GetGenericTypeDefinition(), check))
                        return true;

                type = type.GetTypeInfo().BaseType;
            }
        }
    }
}