namespace Skeleton.Infrastructure.DependencyResolver
{
    using Common;
    using Common.Reflection;
    using LoggerExtension;
    using Microsoft.Practices.Unity;

    internal sealed class CommonModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.AddExtension(new LoggerConstructorInjectionExtension());
            Container.RegisterType<ICacheProvider, MemoryCacheProvider>();
            Container.RegisterType<ITypeAccessorCache, TypeAccessorCache>();
        }
    }
}