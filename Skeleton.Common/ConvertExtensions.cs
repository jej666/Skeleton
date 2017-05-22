using System;
using System.Globalization;

namespace Skeleton.Common
{
    public static class ConvertExtensions
    {
        public static object ChangeType(this object value, Type type, IFormatProvider provider)
        {
            type.ThrowIfNull(nameof(type));

            while (true)
            {
                if (value == null)
                    return null;

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

        public static object ChangeType(this object value, Type type)
        {
            return ChangeType(value, type, CultureInfo.CurrentCulture);
        }

        public static T ChangeType<T>(this object value)
        {
            return (T)ChangeType(value, typeof(T), CultureInfo.CurrentCulture);
        }

        public static object ChangeType(this object value)
        {
            return ChangeType(value, typeof(object), CultureInfo.CurrentCulture);
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
    }
}