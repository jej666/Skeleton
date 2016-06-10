using System.Globalization;

namespace System
{
    public static class StringExtensions
    {
        public static string ConcatWith(this string value, params string[] values)
        {
            value.ThrowIfNullOrEmpty(() => value);

            return string.Concat(value, string.Concat(values));
        }

        public static bool Contains(this string value, string toCheck, StringComparison comp)
        {
            value.ThrowIfNullOrEmpty(() => value);

            return value.IndexOf(toCheck, comp) >= 0;
        }

        public static bool EqualsAny(this string value, StringComparison comparisonType, params string[] values)
        {
            value.ThrowIfNullOrEmpty(() => value);
            values.ThrowIfNull(() => values);

            for (var index = 0; index < values.Length; index++)
            {
                var buffer = values[index];
                if (buffer.Equals(value, comparisonType)) return true;
            }
            return false;
        }

        public static bool EquivalentTo(this string value, string other)
        {
            value.ThrowIfNullOrEmpty(() => value);
            other.ThrowIfNullOrEmpty(() => other);

            return string.Equals(value, other, StringComparison.OrdinalIgnoreCase);
        }

        public static string FormatWith(this string value, params object[] parameters)
        {
            value.ThrowIfNullOrEmpty(() => value);

            return string.Format(CultureInfo.InvariantCulture, value, parameters);
        }

        public static string GetAfter(this string value, string character)
        {
            value.ThrowIfNullOrEmpty(() => value);
            character.ThrowIfNullOrEmpty(() => character);

            var xPos = value.LastIndexOf(character, StringComparison.Ordinal);

            if (xPos == -1)
                return string.Empty;

            var startIndex = xPos + character.Length;
            return startIndex >= value.Length ? string.Empty : value.Substring(startIndex).Trim();
        }

        public static string GetBefore(this string value, string character)
        {
            value.ThrowIfNullOrEmpty(() => value);

            var xPos = value.IndexOf(character, StringComparison.Ordinal);
            return xPos == -1 ? string.Empty : value.Substring(0, xPos);
        }

        public static string SubstringFrom(this string value, int index)
        {
            value.ThrowIfNullOrEmpty(() => value);

            return index < 0 ? value : value.Substring(index, value.Length - index);
        }
    }
}