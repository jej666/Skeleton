using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;
using Skeleton.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Skeleton.Web.Server.Controllers
{
    public class AsyncEntityReaderController<TEntity, TDto> : ControllerBase
        where TEntity : class, IEntity<TEntity>, new()
        where TDto : class, new()
    {
        private readonly IEntityMapper<TEntity, TDto> _mapper;
        private readonly IAsyncEntityReader<TEntity> _reader;

        public AsyncEntityReaderController(
            ILogger logger,
            IAsyncEntityReader<TEntity> reader,
            IEntityMapper<TEntity, TDto> mapper) :
            base(logger)
        {
            _reader = reader;
            _mapper = mapper;
        }

        public IAsyncEntityReader<TEntity> Reader => _reader;

        public IEntityMapper<TEntity, TDto> Mapper => _mapper;

        [HttpGet]
        public virtual async Task<IHttpActionResult> FirstOrDefault(string id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var result = await Reader.FirstOrDefaultAsync(id);

                if (result == null)
                    return NotFound();

                return Ok(Mapper.Map(result));
            });
        }

        [HttpGet]
        public virtual async Task<IHttpActionResult> All()
        {
            return await HandleExceptionAsync(async () =>
            {
                var result = await Reader.FindAsync();

                if (result == null)
                    return NotFound();

                var dtoData = await result
                    .Select(Mapper.Map)
                    .ToListAsync();

                return Ok(dtoData);
            });
        }

        [HttpGet]
        public virtual async Task<IHttpActionResult> Query([FromUri]Query query)
        {
            return await HandleExceptionAsync(async () =>
            {
                var data = await Reader.QueryAsync(query);
                var items = await data
                    .Select(Mapper.Map)
                    .ToListAsync();
                var result = Request.EnrichQueryResult(items, query);

                return Ok(result);
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _reader.Dispose();

            base.Dispose(disposing);
        }
    }
}