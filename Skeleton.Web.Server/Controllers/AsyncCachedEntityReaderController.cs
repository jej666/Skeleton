using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;

namespace Skeleton.Web.Server.Controllers
{
    public class AsyncCachedReadController<TEntity, TDto> :
            AsyncEntityReaderController<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>, new()
        where TDto : class, new()
    {
        public AsyncCachedReadController(
            ILogger logger,
            IAsyncCachedEntityReader<TEntity> reader,
            IEntityMapper<TEntity, TDto> mapper)
            : base(logger, reader, mapper)
        {
        }
    }
}