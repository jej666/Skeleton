using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Web.Server.Controllers
{
    public class AsyncCachedReadController<TEntity, TDto> :
            AsyncReadController<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        public AsyncCachedReadController(
            IAsyncCachedReadRepository<TEntity, TDto> repository)
            : base(repository)
        {
        }
    }
}