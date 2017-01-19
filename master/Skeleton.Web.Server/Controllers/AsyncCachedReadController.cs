using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Repository;
using System.Threading.Tasks;
using System.Web.Http;

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

        [HttpGet]
        public override async Task<IHttpActionResult> Get(string id)
        {
            return await base.Get(id);
        }

        [HttpGet]
        public override async Task<IHttpActionResult> GetAll()
        {
            return await base.GetAll();
        }

        // GET api/<controller>/?pageSize=20&pageNumber=1
        [HttpGet]
        public override async Task<IHttpActionResult> Page(int pageSize, int pageNumber)
        {
            return await base.Page(pageSize, pageNumber);
        }
    }
}