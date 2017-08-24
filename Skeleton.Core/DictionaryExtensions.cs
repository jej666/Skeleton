using System;
using System.Collections.Generic;

namespace Skeleton.Core
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> source,
            TKey key,
            Func<TValue> valueFactory)
        {
            source.ThrowIfNull();
            key.ThrowIfNull();
            valueFactory.ThrowIfNull();

            if (source.ContainsKey(key))
                return source[key];

            var value = valueFactory();

            source.Add(key, value);

            return value;
        }
    }
}