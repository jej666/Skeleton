using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;

namespace Skeleton.Web.Server.Controllers
{
    public class AsyncCachedReadController<TEntity, TDto> :
            AsyncEntityReaderController<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        public AsyncCachedReadController(
            IAsyncCachedEntityReader<TEntity> reader,
            IEntityMapper<TEntity, TDto> mapper)
            : base(reader, mapper)
        {
        }
    }
}