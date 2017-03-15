using Microsoft.Practices.Unity;
using Skeleton.Abstraction.Orm;
using Skeleton.Infrastructure.Orm;

namespace Skeleton.Infrastructure.DependencyInjection
{
    internal sealed class OrmModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container
                .RegisterType(typeof(IEntityReader<>), typeof(EntityReader<>), new HierarchicalLifetimeManager())
                .RegisterType(typeof(IEntityWriter<>), typeof(EntityWriter<>), new HierarchicalLifetimeManager())
                .RegisterType(typeof(ICachedEntityReader<>), typeof(CachedEntityReader<>), new HierarchicalLifetimeManager())
                .RegisterType(typeof(IEntityMapper<,>), typeof(EntityMapper<,>), new HierarchicalLifetimeManager())
                .RegisterType<IStoredProcedureExecutor, StoredProcedureExecutor>();
        }
    }
}