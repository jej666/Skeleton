using Microsoft.Practices.Unity;
using Skeleton.Core.Domain;
using Skeleton.Infrastructure.DependencyResolver.LoggerExtension;
using Skeleton.Infrastructure.Logging;
using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;
using Skeleton.Shared.CommonTypes;
using Skeleton.Shared.CommonTypes.Reflection;

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