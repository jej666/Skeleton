using Microsoft.Practices.Unity;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class ServiceModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            //TODO : test this or scan assembly for convention based registration
            // Container.RegisterType(typeof(IService< , >), typeof(ServiceBase<,>));
        }
    }
}