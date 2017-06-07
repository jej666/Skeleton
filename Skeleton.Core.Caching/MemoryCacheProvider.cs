using Skeleton.Abstraction;
using Skeleton.Common;
using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Skeleton.Core.Caching
{
    public sealed class MemoryCacheProvider :
        HideObjectMethodsBase,
        ICacheProvider
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;

        public T GetOrAdd<T>(string key, Func<T> valueFactory, Action<ICacheConfiguration> configurator)
        {
            key.ThrowIfNullOrEmpty(nameof(key));
            valueFactory.ThrowIfNull(nameof(valueFactory));

            try
            {
                if (Contains(key))
                    return (T)Cache[key];

                var policy = new CachePolicyFactory().Create(configurator);
                var value = valueFactory();

                if (!typeof(T).IsValueType && value == null)
                    return default(T);

                Cache.Add(key, value, policy);

                return value;
            }
            catch (Exception)
            {
                Cache.Remove(key);
                throw;
            }
        }

        public bool Contains(string key)
        {
            return Cache.Contains(key);
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }
    }
}