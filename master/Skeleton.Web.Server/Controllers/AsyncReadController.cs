using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Web.Server.Controllers
{
    public class AsyncReadController<TEntity, TIdentity, TDto> :
            ApiController
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly IAsyncReadRepository<TEntity, TIdentity, TDto> _repository;

        public AsyncReadController(IAsyncReadRepository<TEntity, TIdentity, TDto> repository)
        {
            _repository = repository;
        }

        // GET api/<controller>/5
        public virtual async Task<IHttpActionResult> Get(TIdentity id)
        {
            var result = await _repository.Query.FirstOrDefaultAsync(id);

            if (result == null)
                return NotFound();

            return Ok(_repository.Mapper.Map(result));
        }

        public virtual async Task<IHttpActionResult> Get()
        {
            var result = await _repository.Query.GetAllAsync();

            if (result == null)
                return NotFound();

            var dtoData = result
                .Select(_repository.Mapper.Map)
                .ToList();

            return Ok(dtoData);
        }

        // GET api/<controller>/?pageSize=20&pageNumber=1
        [HttpGet]
        public virtual async Task<IHttpActionResult> Page(int pageSize, int pageNumber)
        {
            var totalCount = await _repository.Query.CountAsync();
            var pagedData = await _repository.Query
                .PageAsync(pageSize, pageNumber);
            var pagedResult = Request.SetPagedResult(
                totalCount,
                pageNumber,
                pageSize,
                pagedData.Select(_repository.Mapper.Map).ToList());

            return Ok(pagedResult);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _repository.Dispose();

            base.Dispose(disposing);
        }
    }
}