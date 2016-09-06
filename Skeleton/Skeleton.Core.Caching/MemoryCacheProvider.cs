using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Skeleton.Abstraction;
using Skeleton.Common;

namespace Skeleton.Core.Caching
{
    public sealed class MemoryCacheProvider :
        HideObjectMethods,
        ICacheProvider
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;

        private readonly Action<ICacheContext> _defaultCacheContext =
            context => context.SetAbsoluteExpiration(TimeSpan.FromSeconds(300));

        public T GetOrAdd<T>(string key, Func<T> valueFactory, Action<ICacheContext> configurator)
        {
            try
            {
                if (Cache.Contains(key))
                    return (T) Cache[key];

                if (configurator == null)
                    configurator = _defaultCacheContext;

                var policy = new CachePolicyFactory().Create(configurator);
                var value = valueFactory();

                Cache.Add(key, value, policy);

                return value;
            }
            catch (Exception)
            {
                Cache.Remove(key);
                throw;
            }
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFactory, Action<ICacheContext> configurator)
        {
            if (configurator == null)
                configurator = _defaultCacheContext;

            var policy = new CachePolicyFactory().Create(configurator);
            var asyncLazyValue = new LazyAsync<T>(valueFactory);
            var existingValue = (LazyAsync<T>) Cache.AddOrGetExisting(key, asyncLazyValue, policy);

            if (existingValue != null)
                asyncLazyValue = existingValue;

            try
            {
                var result = await asyncLazyValue;

                if (asyncLazyValue != Cache.AddOrGetExisting(key, new LazyAsync<T>(valueFactory), policy))
                    return await GetOrAddAsync(key, valueFactory, configurator);
                return result;
            }
            catch (Exception)
            {
                Cache.Remove(key);
                throw;
            }
        }

        public bool Contains<T>(string key)
        {
            return Cache.Contains(key);
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        private class CachePolicyFactory
        {
            private readonly MemoryCacheContext _cacheContext =
                new MemoryCacheContext {CreationTime = DateTimeOffset.UtcNow};

            internal CacheItemPolicy Create(Action<ICacheContext> configurator)
            {
                configurator(_cacheContext);
                var policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = _cacheContext.AbsoluteExpiration
                        .GetValueOrDefault(DateTimeOffset.MaxValue),
                    SlidingExpiration = _cacheContext.SlidingExpiration
                        .GetValueOrDefault(TimeSpan.Zero)
                };
                return policy;
            }
        }
    }
}