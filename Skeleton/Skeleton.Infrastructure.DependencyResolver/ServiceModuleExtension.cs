using Microsoft.Practices.Unity;
using Skeleton.Core.Service;
using Skeleton.Infrastructure.Service;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class ServiceModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType(typeof(IReadService<,>), typeof(ReadService<,>))
                     .RegisterType(typeof(ICrudService<,>), typeof(CrudService<,>))
                     .RegisterType(typeof(ICachedService<,>), typeof(CachedService<,>))
                     .RegisterType(typeof(IReadServiceAsync<,>), typeof(ReadServiceAsync<,>))
                     .RegisterType(typeof(ICrudServiceAsync<,>), typeof(CrudServiceAsync<,>))
                     .RegisterType(typeof(ICachedServiceAsync<,>), typeof(CachedServiceAsync<,>));
        }
    }
}