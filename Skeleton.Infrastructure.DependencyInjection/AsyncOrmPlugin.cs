using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Orm;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Orm;

namespace Skeleton.Infrastructure.DependencyInjection
{
    public sealed class AsyncOrmPlugin : IPlugin
    {
        public void Configure(IBootstrapper host)
        {
            host.RegisterType<IAsyncDatabase, AsyncDatabase>(DependencyLifetime.Scoped)
                .RegisterType(typeof(IAsyncEntityReader<>), typeof(AsyncEntityReader<>), DependencyLifetime.Scoped)
                .RegisterType(typeof(IAsyncEntityWriter<>), typeof(AsyncEntityWriter<>), DependencyLifetime.Scoped) 
                .RegisterType(typeof(IAsyncCachedEntityReader<>), typeof(AsyncCachedEntityReader<>), DependencyLifetime.Scoped)
                .RegisterType<IAsyncStoredProcedureExecutor, AsyncStoredProcedureExecutor>(DependencyLifetime.Scoped);
        }
    }
}