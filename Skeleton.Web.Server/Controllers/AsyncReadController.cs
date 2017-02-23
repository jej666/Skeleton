using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Skeleton.Web.Server.Controllers
{
    public class AsyncReadController<TEntity, TDto> :
            ApiController
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        private readonly IAsyncReadRepository<TEntity, TDto> _repository;

        public AsyncReadController(IAsyncReadRepository<TEntity, TDto> repository)
        {
            repository.ThrowIfNull(() => repository);

            _repository = repository;
        }

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
        [HttpGet]
        public virtual async Task<IHttpActionResult> Get(string id)
        {
            var result = await _repository.Query.FirstOrDefaultAsync(id);

            if (result == null)
                return NotFound();

            return Ok(_repository.Mapper.Map(result));
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [HttpGet]
        public virtual async Task<IHttpActionResult> GetAll()
        {
            var result = await _repository.Query.FindAsync();

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