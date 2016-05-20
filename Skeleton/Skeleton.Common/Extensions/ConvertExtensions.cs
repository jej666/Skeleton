using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Skeleton.Common.Extensions
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

                    if (string.IsNullOrEmpty(s) && toType != typeof(string))
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

                if (toType.IsGenericType && toType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    toType = Nullable.GetUnderlyingType(toType);
                }

                var canConvert = toType.IsValueType && !toType.IsEnum;

                try
                {
                    if (canConvert)
                    {
                        return (T) Convert.ChangeType(value, toType, provider);
                    }
                    return (T) value;
                }
                catch
                {
                    return default(T);
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
                return valueType != null && valueType.Equals(
                    Convert.ChangeType(zeroValue, type, CultureInfo.InvariantCulture));
            }

            if (type == typeof(string))
            {
                return value.Equals(string.Empty) ||
                       value.Equals(" ");
            }

            return false;
        }

        public static int ToBinaryValue(this bool value)
        {
            return value ? 1 : 0;
        }

        public static bool ToBoolean(this int binaryValue)
        {
            return binaryValue == 1;
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