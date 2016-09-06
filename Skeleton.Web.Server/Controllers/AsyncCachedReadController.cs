using System.Threading.Tasks;
using System.Web.Http;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Web.Server.Controllers
{
    public class AsyncCachedReadController<TEntity, TIdentity, TDto> :
            AsyncReadController<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public AsyncCachedReadController(
            IAsyncCachedReadRepository<TEntity, TIdentity, TDto> repository)
            : base(repository)
        {
        }

        // GET api/<controller>/5
        public override async Task<IHttpActionResult> Get(TIdentity id)
        {
            return await base.Get(id);
        }

        public override async Task<IHttpActionResult> Get()
        {
            return await base.Get();
        }

        // GET api/<controller>/?pageSize=20&pageNumber=1
        [HttpGet]
        public override async Task<IHttpActionResult> Page(int pageSize, int pageNumber)
        {
            return await base.Page(pageSize, pageNumber);
        }
    }
}