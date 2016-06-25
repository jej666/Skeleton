using Microsoft.Practices.Unity;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Infrastructure.DependencyResolver
{
    internal sealed class RepositoryModuleExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType(typeof(IReadRepository<,>), typeof(ReadRepository<,>))
                     .RegisterType(typeof(ICrudRepository<,>), typeof(CrudRepository<,>))
                     .RegisterType(typeof(ICachedRepository<,>), typeof(CachedRepository<,>))
                     .RegisterType(typeof(IReadRepositoryAsync<,>), typeof(ReadRepositoryAsync<,>))
                     .RegisterType(typeof(ICrudRepositoryAsync<,>), typeof(CrudRepositoryAsync<,>))
                     .RegisterType(typeof(ICachedRepositoryAsync<,>), typeof(CachedRepositoryAsync<,>));
        }
    }
}