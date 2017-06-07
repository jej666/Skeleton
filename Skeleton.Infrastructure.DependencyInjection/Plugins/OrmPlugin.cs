using Skeleton.Abstraction;
using Skeleton.Abstraction.Orm;
using Skeleton.Abstraction.Startup;
using Skeleton.Infrastructure.Orm;

namespace Skeleton.Infrastructure.DependencyInjection.Plugins
{
    public sealed class OrmPlugin : IPlugin
    {
        public void Configure(IBootstrapper bootstrapper)
        {
            bootstrapper.RegisterType(typeof(IEntityReader<>), typeof(EntityReader<>), DependencyLifetime.Scoped)
                        .RegisterType(typeof(IEntityWriter<>), typeof(EntityWriter<>), DependencyLifetime.Scoped)
                        .RegisterType(typeof(ICachedEntityReader<>), typeof(CachedEntityReader<>), DependencyLifetime.Scoped)
                        .RegisterType(typeof(IEntityMapper<,>), typeof(EntityMapper<,>), DependencyLifetime.Scoped)
                        .RegisterType<IStoredProcedureExecutor, StoredProcedureExecutor>(DependencyLifetime.Scoped);
        }
    }
}