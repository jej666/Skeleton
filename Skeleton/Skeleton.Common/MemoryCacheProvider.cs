namespace Skeleton.Common
{
    using Extensions;
    using System;
    using System.Runtime.Caching;
    using System.Threading.Tasks;

    public class MemoryCacheProvider :
        HideObjectMethods,
        ICacheProvider
    {
        private static ObjectCache cache = MemoryCache.Default;

        private readonly Action<ICacheContext> DefaultCacheContext =
            context => context.SetAbsoluteExpiration(TimeSpan.FromSeconds(300));

        public T GetOrAdd<T>(string key, Func<T> valueFactory, Action<ICacheContext> configurator)
        { 
            if (cache.Contains(key))
                return (T)cache[key];

            if (configurator == null)
                configurator = DefaultCacheContext;

            var policy = new CachePolicyFactory().Create(configurator);          
            var value = valueFactory();

            if (value == null)
                return default(T);

            cache.Add(key, value, policy);

            return value;
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFactory, Action<ICacheContext> configurator)
        {
            if (configurator == null)
                configurator = DefaultCacheContext;

            var policy = new CachePolicyFactory().Create(configurator);

            var asyncLazyValue = new LazyAsync<T>(valueFactory);
            var existingValue = (LazyAsync<T>)cache.AddOrGetExisting(key, asyncLazyValue, policy);

            if (existingValue != null)
            {
                asyncLazyValue = existingValue;
            }

            try
            {
                var result = await asyncLazyValue;

                if (asyncLazyValue != cache.AddOrGetExisting(key, new LazyAsync<T>(valueFactory), policy))
                {
                    return await GetOrAddAsync(key, valueFactory, configurator);
                }
                return result;
            }
            catch (Exception)
            {
                cache.Remove(key);
                throw;
            }
        }

        public bool Contains<T>(string key)
        {
            return cache.Contains(key);  
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

        private class CachePolicyFactory
        {
            private readonly MemoryCacheContext _cacheContext =
                new MemoryCacheContext() { CreationTime = DateTimeOffset.UtcNow };

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