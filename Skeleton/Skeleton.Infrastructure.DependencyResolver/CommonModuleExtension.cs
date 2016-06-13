using Microsoft.Practices.Unity;
using Skeleton.Abstraction;
using Skeleton.Common;
using Skeleton.Common.Reflection;
using Skeleton.Core.Domain;
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
                .RegisterType<ITypeAccessorCache, TypeAccessorCache>()
                .RegisterType<IConfigurationProvider, ConfigurationProvider>()
                .RegisterType(typeof(IEntityMapper<,>),typeof(EntityMapper<,>));
        }
    }
}