using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;
using System.Linq;
using System.Web.Http;

namespace Skeleton.Web.Server.Controllers
{
    public class EntityReaderController<TEntity, TDto> : ControllerBase
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        private readonly IEntityMapper<TEntity, TDto> _mapper;
        private readonly IEntityReader<TEntity> _reader;

        public EntityReaderController(
            ILogger logger,
            IEntityReader<TEntity> reader,
            IEntityMapper<TEntity, TDto> mapper) :
            base(logger)
        {
            _reader = reader;
            _mapper = mapper;
        }

        public IEntityReader<TEntity> Reader => _reader;

        public IEntityMapper<TEntity, TDto> Mapper => _mapper;

        [HttpGet]
        public virtual IHttpActionResult Get(string id)
        {
            return HandleException(() =>
            {
                var result = Reader.FirstOrDefault(id);

                if (result == null)
                    return NotFound();

                return Ok(Mapper.Map(result));
            });
        }

        [HttpGet]
        public virtual IHttpActionResult GetAll()
        {
            return HandleException(() =>
            {
                var allData = Reader
                .Find()
                .Select(Mapper.Map)
                .ToList();

                return Ok(allData);
            });
        }

        [HttpGet]
        public virtual IHttpActionResult Page(int pageSize, int pageNumber)
        {
            return HandleException(() =>
            {
                var totalCount = Reader.Count();
                var pagedData = Reader
                    .Page(pageSize, pageNumber)
                    .Select(_mapper.Map)
                    .ToList();
                var pagedResult = Request.ToPagedResult(
                    totalCount, pageNumber, pageSize, pagedData);

                return Ok(pagedResult);
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