using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Reflection;
using Skeleton.Core.Caching;
using Skeleton.Core.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Logging;

namespace Skeleton.Infrastructure.DependencyInjection
{
    public sealed class CorePlugin : IPlugin
    {
        public void Configure(IBootstrapper host)
        {
            host.RegisterInstance<ILoggerFactory>(new LoggerFactory())
                .RegisterType<IAsyncCacheProvider, AsyncMemoryCacheProvider>()
                .RegisterType<ICacheProvider, MemoryCacheProvider>()
                .RegisterType<IMetadataProvider, MetadataProvider>()
                .RegisterType<IDatabaseConfigurationBuilder, DatabaseConfigurationBuilder>();
        }
    }
}