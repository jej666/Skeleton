using Skeleton.Abstraction;
using Skeleton.Abstraction.Reflection;
using Skeleton.Infrastructure.DependencyInjection;

namespace Skeleton.Tests.Core
{
    public abstract class CoreTestsBase
    {
        private readonly IAppHost _host = new AppHost();

        public IMetadataProvider MetadataProvider => _host.Resolve<IMetadataProvider>();

        public ICacheProvider CacheProvider => _host.Resolve<ICacheProvider>();

        public IAsyncCacheProvider AsyncCacheProvider => _host.Resolve<IAsyncCacheProvider>();
    }
}