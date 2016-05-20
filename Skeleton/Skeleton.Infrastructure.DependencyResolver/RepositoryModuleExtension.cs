using Microsoft.Practices.Unity;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class RepositoryModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType(typeof(IReadOnlyRepository<,>), typeof(ReadOnlyRepositoryBase<,>));
           // Container.RegisterType(typeof(IRepository<,>), typeof(RepositoryBase<,>));
           // Container.RegisterType(typeof(ICachedRepository<,>), typeof(CachedRepositoryBase<,>));
            //   Container.RegisterType(typeof(IReadOnlyRepositoryAsync<,>), typeof(ReadOnlyRepositoryAsyncBase<,>));
            //   Container.RegisterType(typeof(IRepositoryAsync<,>), typeof(RepositoryAsyncBase<,>));
            //   Container.RegisterType(typeof(ICachedRepositoryAsync<,>), typeof(CachedRepositoryAsyncBase<,>));
        }
    }
}