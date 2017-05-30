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
            host.RegisterType<IDatabase, Database>()
                .RegisterType(typeof(IEntityReader<>), typeof(EntityReader<>))
                .RegisterType(typeof(IEntityWriter<>), typeof(EntityWriter<>))
                .RegisterType(typeof(ICachedEntityReader<>), typeof(CachedEntityReader<>))
                .RegisterType(typeof(IEntityMapper<,>), typeof(EntityMapper<,>))
                .RegisterType<IStoredProcedureExecutor, StoredProcedureExecutor>();
        }
    }
}