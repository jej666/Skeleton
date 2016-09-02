using Microsoft.Practices.Unity;
using Skeleton.Abstraction;
using Skeleton.Core.Caching;
using Skeleton.Core.Reflection;
using Skeleton.Infrastructure.DependencyResolver.LoggerExtension;
using Skeleton.Infrastructure.Logging;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class CommonModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            LoggerConfiguration.Configure();

            Container.AddExtension(new LoggerConstructorInjectionExtension())
                .RegisterType<ICacheProvider, MemoryCacheProvider>()
                .RegisterType<IMetadataProvider, MetadataProvider>();
        }
    }
}