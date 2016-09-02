using System.Collections.Generic;

namespace System
{
    public static class EnumExtensions
    {
        public static int FromEnum<T>(this T value) where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(value, default(T)))
                return 0;

            var enumType = typeof(T);

            enumType.ThrowIfNotEnum();

            if (Enum.IsDefined(enumType, value))
            {
                return (int) Enum.ToObject(enumType, value);
            }

            return 0;
        }

        public static T ToEnum<T>(this int value) where T : struct
        {
            return ParseEnum(value, default(T));
        }

        public static T ToEnum<T>(this int value, T defaultValue) where T : struct
        {
            return ParseEnum(value, defaultValue);
        }

        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (value == null)
                return defaultValue;

            int num;

            if (int.TryParse(value, out num))
            {
                if (Enum.IsDefined(typeof(T), num))
                    return (T) Enum.ToObject(typeof(T), num);
            }
            else
            {
                if (Enum.IsDefined(typeof(T), value))
                    return (T) Enum.Parse(typeof(T), value, true);
            }

            return defaultValue;
        }

        private static T ParseEnum<T>(int value, T defaultValue) where T : struct
        {
            var enumType = typeof(T);

            enumType.ThrowIfNotEnum();

            if (Enum.IsDefined(enumType, value))
            {
                return (T) Enum.ToObject(enumType, value);
            }

            return defaultValue;
        }
    }
}