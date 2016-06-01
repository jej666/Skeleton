using Microsoft.Practices.Unity;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class RepositoryModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType(typeof(IReadOnlyRepository<,>), typeof(ReadOnlyRepository<,>))
                     .RegisterType(typeof(IRepository<,>), typeof(Repository<,>))
                     .RegisterType(typeof(ICachedRepository<,>), typeof(CachedRepository<,>))
                     .RegisterType(typeof(IReadOnlyRepositoryAsync<,>), typeof(ReadOnlyRepositoryAsync<,>))
                     .RegisterType(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>))
                     .RegisterType(typeof(ICachedRepositoryAsync<,>), typeof(CachedRepositoryAsync<,>));
        }
    }
}