using System.Web.Http;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Web.Server.Controllers
{
    public class CachedReadController<TEntity, TIdentity, TDto> :
            ReadController<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public CachedReadController(
            ICachedReadRepository<TEntity, TIdentity, TDto> repository)
            : base(repository)
        {
        }

        // GET api/<controller>/5
        // ReSharper disable once RedundantOverriddenMember
        public override IHttpActionResult Get(TIdentity id)
        {
            return base.Get(id);
        }


        // ReSharper disable once RedundantOverriddenMember
        public override IHttpActionResult Get()
        {
            return base.Get();
        }

        // GET api/<controller>/?pageSize=20&pageNumber=1
        [HttpGet]
        public override IHttpActionResult Page(int pageSize, int pageNumber)
        {
            return base.Page(pageSize, pageNumber);
        }
    }
}