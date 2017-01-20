using Skeleton.Abstraction.Domain;

namespace Skeleton.Infrastructure.Repository
{
    internal sealed class AsyncCacheKeyGenerator<TEntity> :
            CacheKeyGenerator<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        internal AsyncCacheKeyGenerator()
        {
            Prefix = "async_";
        }
    }
}