using Microsoft.Practices.Unity;
using Skeleton.Core;
using Skeleton.Core.Reflection;
using Skeleton.Infrastructure.DependencyResolver.LoggerExtension;
using Skeleton.Infrastructure.Logging;
using Skeleton.Shared;
using Skeleton.Shared.CommonTypes;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class CommonModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            LoggerConfiguration.Configure();

            Container.AddExtension(new LoggerConstructorInjectionExtension())
                .RegisterType<ICacheProvider, MemoryCacheProvider>()
                .RegisterType<IMetadataProvider, MetadataProvider>()
                .RegisterType<IConfigurationProvider, ConfigurationProvider>();
        }
    }
}