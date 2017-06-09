using Skeleton.Abstraction;
using Skeleton.Abstraction.Reflection;
using Skeleton.Abstraction.Dependency;
using Skeleton.Infrastructure.Dependency;

namespace Skeleton.Tests.Core
{
    public abstract class CoreTestsBase
    {
        private readonly IBootstrapper _bootstrapper = new Bootstrapper();

        public IMetadataProvider MetadataProvider => _bootstrapper.Container.Resolve<IMetadataProvider>();

        public ICacheProvider CacheProvider => _bootstrapper.Container.Resolve<ICacheProvider>();

        public IAsyncCacheProvider AsyncCacheProvider => _bootstrapper.Container.Resolve<IAsyncCacheProvider>();
    }
}