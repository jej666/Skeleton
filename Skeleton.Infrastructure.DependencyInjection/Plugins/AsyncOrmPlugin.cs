using Skeleton.Abstraction;
using Skeleton.Abstraction.Orm;
using Skeleton.Abstraction.Startup;
using Skeleton.Infrastructure.Orm;

namespace Skeleton.Infrastructure.DependencyInjection.Plugins
{
    public sealed class AsyncOrmPlugin : IPlugin
    {
        public void Configure(IBootstrapper bootstrapper)
        {
            bootstrapper.RegisterType(typeof(IAsyncEntityReader<>), typeof(AsyncEntityReader<>), DependencyLifetime.Scoped)
                        .RegisterType(typeof(IAsyncEntityWriter<>), typeof(AsyncEntityWriter<>), DependencyLifetime.Scoped)
                        .RegisterType(typeof(IAsyncCachedEntityReader<>), typeof(AsyncCachedEntityReader<>), DependencyLifetime.Scoped)
                        .RegisterType<IAsyncStoredProcedureExecutor, AsyncStoredProcedureExecutor>(DependencyLifetime.Scoped);
        }
    }
}