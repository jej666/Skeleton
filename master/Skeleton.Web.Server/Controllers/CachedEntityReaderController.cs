using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;

namespace Skeleton.Web.Server.Controllers
{
    public class CachedEntityReaderController<TEntity, TDto> :
            EntityReaderController<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        public CachedEntityReaderController(
            ICachedEntityReader<TEntity> reader,
            IEntityMapper<TEntity, TDto> mapper)
            : base(reader, mapper)
        {
        }
    }
}