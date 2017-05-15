using Skeleton.Abstraction;
using Skeleton.Common;
using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Skeleton.Core.Caching
{
    public sealed class AsyncMemoryCacheProvider :
        HideObjectMethodsBase,
        IAsyncCacheProvider
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;
        const string KeyPrefix = "async_";

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFactory, Action<ICacheContext> configurator)
        {
            var policy = new CachePolicyFactory().Create(configurator);
            var asyncLazyValue = new LazyAsync<T>(valueFactory);
            var existingValue = (LazyAsync<T>)Cache.AddOrGetExisting(KeyPrefix + key, asyncLazyValue, policy);

            if (existingValue != null)
                asyncLazyValue = existingValue;

            try
            {
                var result = await asyncLazyValue;

                if (asyncLazyValue != Cache.AddOrGetExisting(KeyPrefix + key, new LazyAsync<T>(valueFactory), policy))
                    return await GetOrAddAsync(KeyPrefix + key, valueFactory, configurator);

                return result;
            }
            catch (Exception)
            {
                await RemoveAsync(KeyPrefix + key);
                throw;
            }
        }

        public async Task<bool> ContainsAsync(string key)
        {
            return await Task.Factory.StartNew(() => Cache.Contains(KeyPrefix + key));
        }

        public async Task RemoveAsync(string key)
        {
            await Task.Factory.StartNew(() => Cache.Remove(KeyPrefix + key));
        }
    }
}