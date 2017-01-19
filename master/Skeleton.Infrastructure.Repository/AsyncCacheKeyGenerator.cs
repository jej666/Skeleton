using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class AsyncCacheKeyGenerator<TEntity> :
            CacheKeyGenerator<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        public AsyncCacheKeyGenerator()
        {
            Prefix = "async_";
        }
    }
}