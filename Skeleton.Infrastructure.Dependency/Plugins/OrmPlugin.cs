using Skeleton.Abstraction.Dependency;
using Skeleton.Abstraction.Orm;
using Skeleton.Core;
using Skeleton.Infrastructure.Orm;

namespace Skeleton.Infrastructure.Dependency.Plugins
{
    public sealed class OrmPlugin : IPlugin
    {
        public void Configure(IDependencyContainer container)
        {
            container.ThrowIfNull();

            container.Register
                .Type(typeof(IEntityReader<>), typeof(EntityReader<>), DependencyLifetime.Scoped)
                .Type(typeof(IEntityWriter<>), typeof(EntityWriter<>), DependencyLifetime.Scoped)
                .Type(typeof(ICachedEntityReader<>), typeof(CachedEntityReader<>), DependencyLifetime.Scoped)
                .Type(typeof(IEntityMapper<,>), typeof(EntityMapper<,>), DependencyLifetime.Scoped)
                .Type<IStoredProcedureExecutor, StoredProcedureExecutor>(DependencyLifetime.Scoped)
                .Type(typeof(IAsyncEntityReader<>), typeof(AsyncEntityReader<>), DependencyLifetime.Scoped)
                .Type(typeof(IAsyncEntityWriter<>), typeof(AsyncEntityWriter<>), DependencyLifetime.Scoped)
                .Type(typeof(IAsyncCachedEntityReader<>), typeof(AsyncCachedEntityReader<>), DependencyLifetime.Scoped)
                .Type<IAsyncStoredProcedureExecutor, AsyncStoredProcedureExecutor>(DependencyLifetime.Scoped);
        }
    }
}