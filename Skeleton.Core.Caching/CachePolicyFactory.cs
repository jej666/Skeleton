using Skeleton.Abstraction;
using System;
using System.Runtime.Caching;

namespace Skeleton.Core.Caching
{
    internal sealed class CachePolicyFactory
    {
        private readonly Action<ICacheContext> _defaultCacheContext =
            context => context.SetAbsoluteExpiration(TimeSpan.FromSeconds(300));

        private readonly MemoryCacheContext _cacheContext =
            new MemoryCacheContext
            {
                CreationTime = DateTimeOffset.UtcNow
            };

        internal CacheItemPolicy Create(Action<ICacheContext> configurator)
        {
            if (configurator == null)
                configurator = _defaultCacheContext;

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