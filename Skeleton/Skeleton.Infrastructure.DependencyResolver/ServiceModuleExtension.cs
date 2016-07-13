using Microsoft.Practices.Unity;
using Skeleton.Core.Service;
using Skeleton.Infrastructure.Service;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class ServiceModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container
                .RegisterType(typeof(IEntityMapper<,,>), typeof(EntityMapper<,,>))
                .RegisterType(typeof(IReadService<,,>), typeof(ReadService<,,>))
                .RegisterType(typeof(ICrudService<,,>), typeof(CrudService<,,>))
                .RegisterType(typeof(ICachedReadService<,,>), typeof(CachedReadService<,,>))
                .RegisterType(typeof(IAsyncReadService<,,>), typeof(AsyncReadService<,,>))
                .RegisterType(typeof(IAsyncCrudService<,,>), typeof(AsyncCrudService<,,>))
                .RegisterType(typeof(IAsyncCachedReadService<,,>), typeof(AsyncCachedReadService<,,>));
        }
    }
}