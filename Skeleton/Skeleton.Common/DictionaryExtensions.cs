using System.Text;

namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static T GetValue<T>(
            this IDictionary<string, object> source,
            string key)
        {
            source.ThrowIfNull(() => source);

            var result = default(T);

            if (!source.ContainsKey(key)) return result;

            var value = source[key];

            if (value == null) return result;

            if (value.GetType() != typeof(T))
            {
                result = value.ChangeType<T>();
            }
            else
            {
                result = (T) value;
            }

            return result;
        }

        public static void SetValue(
            this IDictionary<string, object> source,
            string key,
            object value)
        {
            source.ThrowIfNull(() => source);

            if (source.ContainsKey(key))
                source[key] = value;
            else
                source.Add(key, value);
        }

        public static string ToString<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            string keyValueSeparator,
            string sequenceSeparator)
        {
            var stringBuilder = new StringBuilder();
            dictionary.ForEach(
                x => stringBuilder.AppendFormat("{0}{1}{2}{3}",
                    x.Key.ToString(),
                    keyValueSeparator,
                    x.Value.ToString(),
                    sequenceSeparator));

            return stringBuilder.ToString(0,
                stringBuilder.Length - sequenceSeparator.Length);
        }
    }
}