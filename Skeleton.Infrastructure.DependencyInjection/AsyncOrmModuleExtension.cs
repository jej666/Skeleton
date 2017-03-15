using Microsoft.Practices.Unity;
using Skeleton.Abstraction.Orm;
using Skeleton.Infrastructure.Orm;

namespace Skeleton.Infrastructure.DependencyInjection
{
    internal sealed class AsyncOrmModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container
                .RegisterType(typeof(IAsyncEntityReader<>), typeof(AsyncEntityReader<>), new HierarchicalLifetimeManager())
                .RegisterType(typeof(IAsyncEntityWriter<>), typeof(AsyncEntityWriter<>), new HierarchicalLifetimeManager())
                .RegisterType(typeof(IAsyncCachedEntityReader<>), typeof(AsyncCachedEntityReader<>), new HierarchicalLifetimeManager())
                .RegisterType<IAsyncStoredProcedureExecutor, AsyncStoredProcedureExecutor>();
        }
    }
}