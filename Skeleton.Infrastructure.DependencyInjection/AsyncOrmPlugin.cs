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
            host.RegisterType<IAsyncDatabase, AsyncDatabase>()
                .RegisterType(typeof(IAsyncEntityReader<>), typeof(AsyncEntityReader<>))
                .RegisterType(typeof(IAsyncEntityWriter<>), typeof(AsyncEntityWriter<>)) 
                .RegisterType(typeof(IAsyncCachedEntityReader<>), typeof(AsyncCachedEntityReader<>))
                .RegisterType<IAsyncStoredProcedureExecutor, AsyncStoredProcedureExecutor>();
        }
    }
}