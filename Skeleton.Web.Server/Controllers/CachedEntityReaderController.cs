using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;

namespace Skeleton.Web.Server.Controllers
{
    public class CachedEntityReaderController<TEntity, TDto> :
            EntityReaderController<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>, new()
        where TDto : class, new()
    {
        public CachedEntityReaderController(
            ILogger logger,
            ICachedEntityReader<TEntity> reader,
            IEntityMapper<TEntity, TDto> mapper)
            : base(logger, reader, mapper)
        {
        }
    }
}