using Skeleton.Abstraction;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class AsyncCacheKeyGenerator<TEntity, TIdentity> :
            CacheKeyGenerator<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        public AsyncCacheKeyGenerator()
        {
            Prefix = "async_";
        }
    }
}