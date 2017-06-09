using Skeleton.Abstraction;
using Skeleton.Abstraction.Dependency;
using Skeleton.Core.Caching;

namespace Skeleton.Infrastructure.Dependency.Plugins
{
    public sealed class CachePlugin : IPlugin
    {
        public void Configure(IDependencyContainer container)
        {
            container.Register
                .Type<IAsyncCacheProvider, AsyncMemoryCacheProvider>()
                .Type<ICacheProvider, MemoryCacheProvider>();
        }
    }
}