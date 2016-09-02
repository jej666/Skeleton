using Microsoft.Practices.Unity;
using Skeleton.Abstraction.Repository;
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
                .RegisterType(typeof(IAsyncCachedEntityReader<,>), typeof(AsyncCachedEntityReader<,>))
                .RegisterType(typeof(IEntityMapper<,,>), typeof(EntityMapper<,,>))
                .RegisterType(typeof(IReadRepository<,,>), typeof(ReadRepository<,,>))
                .RegisterType(typeof(ICrudRepository<,,>), typeof(CrudRepository<,,>))
                .RegisterType(typeof(ICachedReadRepository<,,>), typeof(CachedReadRepository<,,>))
                .RegisterType(typeof(IAsyncReadRepository<,,>), typeof(AsyncReadRepository<,,>))
                .RegisterType(typeof(IAsyncCrudRepository<,,>), typeof(AsyncCrudRepository<,,>))
                .RegisterType(typeof(IAsyncCachedReadRepository<,,>), typeof(AsyncCachedReadRepository<,,>));
        }
    }
}