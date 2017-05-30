using Skeleton.Abstraction;
using Skeleton.Abstraction.Reflection;
using Skeleton.Infrastructure.DependencyInjection;

namespace Skeleton.Tests.Core
{
    public abstract class CoreTestsBase
    {
        private readonly IBootstrapper _host = new Bootstrapper();

        public IMetadataProvider MetadataProvider => _host.Resolve<IMetadataProvider>();

        public ICacheProvider CacheProvider => _host.Resolve<ICacheProvider>();

        public IAsyncCacheProvider AsyncCacheProvider => _host.Resolve<IAsyncCacheProvider>();
    }
}