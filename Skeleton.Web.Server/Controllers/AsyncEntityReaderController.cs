using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;
using Skeleton.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Skeleton.Web.Server.Controllers
{
    public class AsyncEntityReaderController<TEntity, TDto> : ApiController
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        private readonly IEntityMapper<TEntity, TDto> _mapper;
        private readonly IAsyncEntityReader<TEntity> _reader;

        public AsyncEntityReaderController(
            IAsyncEntityReader<TEntity> reader,
            IEntityMapper<TEntity, TDto> mapper)
        {
            _reader = reader;
            _mapper = mapper;
        }

        public virtual IAsyncEntityReader<TEntity> Reader => _reader;

        public IEntityMapper<TEntity, TDto> Mapper => _mapper;

        [HttpGet]
        public virtual async Task<IHttpActionResult> Get(string id)
        {
            var result = await Reader.FirstOrDefaultAsync(id);

            if (result == null)
                return NotFound();

            return Ok(Mapper.Map(result));
        }

        [HttpGet]
        public virtual async Task<IHttpActionResult> GetAll()
        {
            var result = await Reader.FindAsync();

            if (result == null)
                return NotFound();

            var dtoData = await result
                .Select(Mapper.Map)
                .ToListAsync();

            return Ok(dtoData);
        }

        [HttpGet]
        public virtual async Task<IHttpActionResult> Page(int pageSize, int pageNumber)
        {
            var totalCount = await Reader.CountAsync();
            var pagedData = await Reader
                .PageAsync(pageSize, pageNumber);
            var dtoData = await pagedData
                .Select(Mapper.Map)
                .ToListAsync();
            var pagedResult = Request.ToPagedResult(
                totalCount,
                pageNumber,
                pageSize,
                dtoData);

            return Ok(pagedResult);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _reader.Dispose();

            base.Dispose(disposing);
        }
    }
}