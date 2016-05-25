using Microsoft.Practices.Unity;
using Skeleton.Common;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.DependencyResolver.LoggerExtension;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class CommonModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.AddExtension(new LoggerConstructorInjectionExtension());
            Container.RegisterType<ICacheProvider, MemoryCacheProvider>();
            Container.RegisterType<ITypeAccessorCache, TypeAccessorCache>();
            Container.RegisterType<IConfigurationProvider, ConfigurationProvider>();
        }
    }
}