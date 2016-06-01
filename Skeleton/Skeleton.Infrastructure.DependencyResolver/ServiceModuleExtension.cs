using Microsoft.Practices.Unity;
using Skeleton.Core.Service;
using Skeleton.Infrastructure.Service;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class ServiceModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType(typeof(IReadOnlyService<,>), typeof(ReadOnlyService<,>))
                     .RegisterType(typeof(IService<,>), typeof(Service<,>))
                     .RegisterType(typeof(ICachedService<,>), typeof(CachedService<,>))
                     .RegisterType(typeof(IReadOnlyServiceAsync<,>), typeof(ReadOnlyServiceAsync<,>))
                     .RegisterType(typeof(IServiceAsync<,>), typeof(ServiceAsync<,>))
                     .RegisterType(typeof(ICachedServiceAsync<,>), typeof(CachedServiceAsync<,>));
        }
    }
}