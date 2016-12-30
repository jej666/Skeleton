using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Skeleton.Common
{
    public static class ConvertExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static T ChangeType<T>(this object value, IFormatProvider provider)
        {
            while (true)
            {
                var toType = typeof(T);

                if (value == null) return default(T);

                var s = value as string;
                if (s != null)
                {
                    if (toType == typeof(Guid))
                    {
                        value = new Guid(Convert.ToString(value, provider));
                        continue;
                    }

                    if (string.IsNullOrEmpty(s) && (toType != typeof(string)))
                    {
                        value = null;
                        continue;
                    }
                }
                else
                {
                    if (typeof(T) == typeof(string))
                    {
                        value = Convert.ToString(value, provider);
                        continue;
                    }
                }

                if (toType.IsGenericType && (toType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    toType = Nullable.GetUnderlyingType(toType);

                var canConvert = toType.IsValueType && !toType.IsEnum;

                try
                {
                    if (canConvert)
                        return (T) Convert.ChangeType(value, toType, provider);
                    return (T) value;
                }
                catch
                {
                    return default(T);
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static object ChangeType(this object value, Type type, IFormatProvider provider)
        {
            type.ThrowIfNull(() => type);

            while (true)
            {
                if (value == null) return null;

                var s = value as string;
                if (s != null)
                {
                    if (type == typeof(Guid))
                    {
                        value = new Guid(Convert.ToString(value, provider));
                        continue;
                    }

                    if (string.IsNullOrEmpty(s) && (type != typeof(string)))
                    {
                        value = null;
                        continue;
                    }
                }
                else
                {
                    if (type == typeof(string))
                    {
                        value = Convert.ToString(value, provider);
                        continue;
                    }
                }

                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    type = Nullable.GetUnderlyingType(type);

                var canConvert = type.IsValueType && !type.IsEnum;

                try
                {
                    return canConvert ? Convert.ChangeType(value, type, provider) : value;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static T ChangeType<T>(this object value)
        {
            return ChangeType<T>(value, CultureInfo.CurrentCulture);
        }

        public static object ChangeType(this object value)
        {
            return ChangeType<object>(value);
        }

        public static bool IsNotZeroOrEmpty(this object value)
        {
            return !value.IsZeroOrEmpty();
        }

        public static bool IsZeroOrEmpty(this object value)
        {
            if (value == null)
                return true;

            var type = value.GetType();

            if (type.IsValueType)
            {
                object zeroValue = 0;
                var valueType = value as ValueType;
                return (valueType != null) && valueType.Equals(
                           Convert.ChangeType(zeroValue, type, CultureInfo.InvariantCulture));
            }

            if (type == typeof(string))
                return value.Equals(string.Empty) ||
                       value.Equals(" ");

            return false;
        }

        public static object TrimIfNeeded(this object item)
        {
            return item is string
                ? item.SafeTrimEnd()
                : item;
        }

        private static string SafeTrimEnd(this object value)
        {
            return value == null
                ? string.Empty
                : Convert.ToString(value, CultureInfo.InvariantCulture).TrimEnd();
        }
    }
}