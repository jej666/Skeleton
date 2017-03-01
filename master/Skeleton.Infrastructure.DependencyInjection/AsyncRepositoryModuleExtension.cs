using Microsoft.Practices.Unity;
using Skeleton.Abstraction.Repository;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Infrastructure.DependencyInjection
{
    internal sealed class AsyncRepositoryModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container
                .RegisterType(typeof(IAsyncEntityReader<>), typeof(AsyncEntityReader<>))
                .RegisterType(typeof(IAsyncEntityWriter<>), typeof(AsyncEntityPersistor<>))
                .RegisterType(typeof(IAsyncCachedEntityReader<>), typeof(AsyncCachedEntityReader<>))
                .RegisterType(typeof(IAsyncReadRepository<,>), typeof(AsyncReadRepository<,>), new HierarchicalLifetimeManager())
                .RegisterType(typeof(IAsyncCrudRepository<,>), typeof(AsyncCrudRepository<,>), new HierarchicalLifetimeManager())
                .RegisterType(typeof(IAsyncCachedReadRepository<,>), typeof(AsyncCachedReadRepository<,>), new HierarchicalLifetimeManager())
                .RegisterType<IAsyncStoredProcedureExecutor, AsyncStoredProcedureExecutor>();
        }
    }
}