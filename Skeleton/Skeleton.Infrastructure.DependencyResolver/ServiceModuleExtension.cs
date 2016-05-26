using Microsoft.Practices.Unity;
using Skeleton.Core.Service;
using Skeleton.Infrastructure.Service;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class ServiceModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType(typeof(IReadOnlyService<,>), typeof(ReadOnlyService<,>));
            Container.RegisterType(typeof(IService<,>), typeof(Service<,>));
        }
    }
}