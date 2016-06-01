using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Skeleton.Common.Extensions
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

        public static Assembly GetAssembly(this Type source)
        {
            return source.GetTypeInfo().Assembly;
        }

        public static bool IsArray(this Type source)
        {
            return source.GetTypeInfo().BaseType == typeof(Array);
        }

        public static bool IsCollection(this Type source)
        {
            var collectionType = typeof(ICollection<>);

            return source.GetTypeInfo().IsGenericType && source
                .GetInterfaces()
                .Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == collectionType);
        }

        public static bool IsEnumerable(this Type source)
        {
            var enumerableType = typeof(IEnumerable<>);

            return source.GetTypeInfo().IsGenericType && source.GetGenericTypeDefinition() == enumerableType;
        }

        public static TypeCode GetTypeCode(this Type type)
        {
            if (type == typeof(bool))
                return TypeCode.Boolean;
            else if (type == typeof(char))
                return TypeCode.Char;
            else if (type == typeof(sbyte))
                return TypeCode.SByte;
            else if (type == typeof(byte))
                return TypeCode.Byte;
            else if (type == typeof(short))
                return TypeCode.Int16;
            else if (type == typeof(ushort))
                return TypeCode.UInt16;
            else if (type == typeof(int))
                return TypeCode.Int32;
            else if (type == typeof(uint))
                return TypeCode.UInt32;
            else if (type == typeof(long))
                return TypeCode.Int64;
            else if (type == typeof(ulong))
                return TypeCode.UInt64;
            else if (type == typeof(float))
                return TypeCode.Single;
            else if (type == typeof(double))
                return TypeCode.Double;
            else if (type == typeof(decimal))
                return TypeCode.Decimal;
            else if (type == typeof(DateTime))
                return TypeCode.DateTime;
            else if (type == typeof(string))
                return TypeCode.String;
            else if (type.GetTypeInfo().IsEnum)
                return GetTypeCode(Enum.GetUnderlyingType(type));
            else
                return TypeCode.Object;
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

        public static bool IsInstantiatableType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("t");

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
            {
                throw new ArgumentNullException("t");
            }

            var hasDefaultConstructor =
                type.GetTypeInfo().DeclaredConstructors.Any(ctor => !ctor.GetParameters().Any());

            return hasDefaultConstructor;
        }

        public static bool IsAssignable(this Type to, Type from)
        {
            if (to == null)
            {
                throw new ArgumentNullException("to");
            }

            if (to.IsAssignableFrom(from))
            {
                return true;
            }

            if (to.GetTypeInfo().IsGenericType && from.GetTypeInfo().IsGenericTypeDefinition)
            {
                return to.IsAssignableFrom(from.MakeGenericType(to.GetGenericArguments()));
            }

            return false;
        }

        public static bool IsSubClass(this Type type, Type check)
        {
            if (type == null || check == null)
            {
                return false;
            }

            if (type == check)
            {
                return true;
            }

            if (check.GetTypeInfo().IsInterface)
            {
                foreach (Type t in type.GetInterfaces())
                {
                    if (IsSubClass(t, check)) return true;
                }
            }
            if (type.GetTypeInfo().IsGenericType && !type.GetTypeInfo().IsGenericTypeDefinition)
            {
                if (IsSubClass(type.GetGenericTypeDefinition(), check))
                    return true;
            }
            return IsSubClass(type.GetTypeInfo().BaseType, check);
        }
    }
}