using Microsoft.Practices.Unity;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class RepositoryModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType(typeof(IEntityReader<,>), typeof(EntityReader<,>))
                .RegisterType(typeof(IEntityPersitor<,>), typeof(EntityPersistor<,>))
                .RegisterType(typeof(ICachedEntityReader<,>), typeof(CachedEntityReader<,>))
                .RegisterType(typeof(IAsyncEntityReader<,>), typeof(AsyncEntityReader<,>))
                .RegisterType(typeof(IAsyncEntityPersistor<,>), typeof(AsyncEntityPersistor<,>))
                .RegisterType(typeof(IAsyncCachedEntityReader<,>), typeof(AsyncCachedEntityReader<,>));
        }
    }
}