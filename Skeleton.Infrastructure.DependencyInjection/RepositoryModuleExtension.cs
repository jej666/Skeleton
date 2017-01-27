using Microsoft.Practices.Unity;
using Skeleton.Abstraction.Repository;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Infrastructure.DependencyInjection
{
    internal sealed class RepositoryModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType(typeof(IEntityReader<>), typeof(EntityReader<>))
                .RegisterType(typeof(IEntityWriter<>), typeof(EntityWriter<>))
                .RegisterType(typeof(ICachedEntityReader<>), typeof(CachedEntityReader<>))
                .RegisterType(typeof(IEntityMapper<,>), typeof(EntityMapper<,>))
                .RegisterType(typeof(IReadRepository<,>), typeof(ReadRepository<,>))
                .RegisterType(typeof(ICrudRepository<,>), typeof(CrudRepository<,>))
                .RegisterType(typeof(ICachedReadRepository<,>), typeof(CachedReadRepository<,>))
                .RegisterType<IStoredProcedureExecutor, StoredProcedureExecutor>();
        }
    }
}