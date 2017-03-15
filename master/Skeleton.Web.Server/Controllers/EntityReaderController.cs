using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;
using System.Linq;
using System.Web.Http;

namespace Skeleton.Web.Server.Controllers
{
    public class EntityReaderController<TEntity, TDto> : ApiController
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        private readonly IEntityMapper<TEntity, TDto> _mapper;
        private readonly IEntityReader<TEntity> _reader;

        public EntityReaderController(
            IEntityReader<TEntity> reader,
            IEntityMapper<TEntity, TDto> mapper)
        {
            _reader = reader;
            _mapper = mapper;
        }

        public virtual IEntityReader<TEntity> Reader => _reader;

        public IEntityMapper<TEntity, TDto> Mapper => _mapper;

        [HttpGet]
        public virtual IHttpActionResult Get(string id)
        {
            var result = Reader.FirstOrDefault(id);

            if (result == null)
                return NotFound();

            return Ok(Mapper.Map(result));
        }

        [HttpGet]
        public virtual IHttpActionResult GetAll()
        {
            var allData = Reader
                .Find()
                .Select(Mapper.Map)
                .ToList();

            return Ok(allData);
        }

        [HttpGet]
        public virtual IHttpActionResult Page(int pageSize, int pageNumber)
        {
            var totalCount = _reader.Count();
            var pagedData = _reader
                .Page(pageSize, pageNumber)
                .Select(_mapper.Map)
                .ToList();
            var pagedResult = Request.ToPagedResult(
                totalCount, pageNumber, pageSize, pagedData);

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