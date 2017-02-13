using System;
using System.Collections.Generic;
using System.Globalization;
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

            source.Add(key, value);

            return value;
        }
    }
}