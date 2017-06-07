using Skeleton.Abstraction;
using Skeleton.Abstraction.Startup;
using Skeleton.Core.Caching;

namespace Skeleton.Infrastructure.DependencyInjection.Plugins
{
    public sealed class CachePlugin : IPlugin
    {
        public void Configure(IBootstrapper bootstrapper)
        {
            bootstrapper.RegisterType<IAsyncCacheProvider, AsyncMemoryCacheProvider>()
                        .RegisterType<ICacheProvider, MemoryCacheProvider>();
        }
    }
}