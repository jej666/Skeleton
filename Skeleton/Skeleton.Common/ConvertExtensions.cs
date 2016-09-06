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

        public static int? ToInt(this string str, NumberStyles ns = NumberStyles.Integer, CultureInfo ci = null)
        {
            int result;
            if (int.TryParse(str, ns, ci ?? CultureInfo.CurrentCulture, out result))
                return result;
            return null;
        }

        public static long? ToLong(this string str, NumberStyles ns = NumberStyles.Integer, CultureInfo ci = null)
        {
            long result;
            if (long.TryParse(str, ns, ci ?? CultureInfo.CurrentCulture, out result))
                return result;
            return null;
        }

        public static short? ToShort(this string str, NumberStyles ns = NumberStyles.Integer, CultureInfo ci = null)
        {
            short result;
            if (short.TryParse(str, ns, ci ?? CultureInfo.CurrentCulture, out result))
                return result;
            return null;
        }

        public static float? ToFloat(this string str, NumberStyles ns = NumberStyles.Float | NumberStyles.AllowThousands,
            CultureInfo ci = null)
        {
            float result;
            if (float.TryParse(str, ns, ci ?? CultureInfo.CurrentCulture, out result))
                return result;
            return null;
        }

        public static double? ToDouble(this string str,
            NumberStyles ns = NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo ci = null)
        {
            double result;
            if (double.TryParse(str, ns, ci ?? CultureInfo.CurrentCulture, out result))
                return result;
            return null;
        }

        public static decimal? ToDecimal(this string str, NumberStyles ns = NumberStyles.Number, CultureInfo ci = null)
        {
            decimal result;
            if (decimal.TryParse(str, ns, ci ?? CultureInfo.CurrentCulture, out result))
                return result;
            return null;
        }

        public static int ToInt(this string str, string error)
        {
            int result;
            if (int.TryParse(str, out result))
                return result;

            throw new FormatException(error);
        }

        public static long ToLong(this string str, string error)
        {
            long result;
            if (long.TryParse(str, out result))
                return result;

            throw new FormatException(error);
        }

        public static short ToShort(this string str, string error)
        {
            short result;
            if (short.TryParse(str, out result))
                return result;

            throw new FormatException(error);
        }

        public static float? ToFloat(this string str, string error)
        {
            float result;
            if (float.TryParse(str, out result))
                return result;

            throw new FormatException(error);
        }

        public static double ToDouble(this string str, string error)
        {
            double result;
            if (double.TryParse(str, out result))
                return result;

            throw new FormatException(error);
        }

        public static decimal ToDecimal(this string str, string error)
        {
            decimal result;
            if (decimal.TryParse(str, out result))
                return result;

            throw new FormatException(error);
        }
    }
}