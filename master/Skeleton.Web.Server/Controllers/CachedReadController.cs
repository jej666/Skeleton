using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Web.Server.Controllers
{
    public class CachedReadController<TEntity, TDto> :
            ReadController<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        public CachedReadController(
            ICachedReadRepository<TEntity, TDto> repository)
            : base(repository)
        {
        }
    }
}