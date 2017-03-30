using System;
using System.Collections.Generic;

namespace Skeleton.Common
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> source,
            TKey key,
            Func<TValue> valueFactory)
        {
            source.ThrowIfNull(nameof(source));
            key.ThrowIfNull(nameof(key));
            valueFactory.ThrowIfNull(nameof(valueFactory));

            if (source.ContainsKey(key))
                return source[key];

            var value = valueFactory();

            source.Add(key, value);

            return value;
        }
    }
}