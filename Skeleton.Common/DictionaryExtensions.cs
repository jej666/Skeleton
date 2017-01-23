using System;
using System.Collections.Generic;
using System.Text;

namespace Skeleton.Common
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> source,
            TKey key,
            Func<TValue> valueFactory)
        {
            source.ThrowIfNull(() => source);
            key.ThrowIfNull(() => key);
            valueFactory.ThrowIfNull(() => valueFactory);

            if (source.ContainsKey(key))
                return source[key];

            var value = valueFactory();

            source.Add(key,value);

            return value;
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