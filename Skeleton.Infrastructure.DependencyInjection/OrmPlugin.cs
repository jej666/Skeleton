using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Orm;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Orm;

namespace Skeleton.Infrastructure.DependencyInjection
{
    public sealed class OrmPlugin : IPlugin
    {
        public void Configure(IBootstrapper host)
        {
            host.RegisterType<IDatabase, Database>(DependencyLifetime.Scoped)
                .RegisterType(typeof(IEntityReader<>), typeof(EntityReader<>), DependencyLifetime.Scoped)
                .RegisterType(typeof(IEntityWriter<>), typeof(EntityWriter<>), DependencyLifetime.Scoped)
                .RegisterType(typeof(ICachedEntityReader<>), typeof(CachedEntityReader<>), DependencyLifetime.Scoped)
                .RegisterType(typeof(IEntityMapper<,>), typeof(EntityMapper<,>), DependencyLifetime.Scoped)
                .RegisterType<IStoredProcedureExecutor, StoredProcedureExecutor>(DependencyLifetime.Scoped);
        }
    }
}