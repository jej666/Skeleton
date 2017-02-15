using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Repository;
using System.Web.Http;

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

        // ReSharper disable once RedundantOverriddenMember
        [HttpGet]
        public override IHttpActionResult Get(string id)
        {
            return base.Get(id);
        }

        // ReSharper disable once RedundantOverriddenMember
        [HttpGet]
        public override IHttpActionResult GetAll()
        {
            return base.GetAll();
        }

        // GET api/<controller>/?pageSize=20&pageNumber=1
        [HttpGet]
        public override IHttpActionResult Page(int pageSize, int pageNumber)
        {
            return base.Page(pageSize, pageNumber);
        }
    }
}