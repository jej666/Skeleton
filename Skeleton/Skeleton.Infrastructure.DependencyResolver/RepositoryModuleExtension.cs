using Microsoft.Practices.Unity;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class RepositoryModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType(typeof(IReadOnlyRepository<,>), typeof(ReadOnlyRepository<,>));
            Container.RegisterType(typeof(IRepository<,>), typeof(Repository<,>));
            Container.RegisterType(typeof(ICachedRepository<,>), typeof(CachedRepository<,>));
            Container.RegisterType(typeof(IReadOnlyRepositoryAsync<,>), typeof(ReadOnlyRepositoryAsync<,>));
            Container.RegisterType(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>));
            Container.RegisterType(typeof(ICachedRepositoryAsync<,>), typeof(CachedRepositoryAsync<,>));
        }
    }
}