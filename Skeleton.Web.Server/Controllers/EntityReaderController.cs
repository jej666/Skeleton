using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;
using Skeleton.Core;
using Skeleton.Web.Server.Helpers;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Skeleton.Web.Server.Controllers
{
    public class EntityReaderController<TEntity, TDto> : ControllerBase
        where TEntity : class, IEntity<TEntity>, new()
        where TDto : class, new()
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
        public virtual IHttpActionResult FirstOrDefault(string id)
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
        public virtual IHttpActionResult All()
        {
            return HandleException(() =>
            {
                var result = Reader.Find();

                if (result == null)
                    return NotFound();

                 var items = result.Select(Mapper.Map)
                                   .ToList();
               
                return Ok(items);
            });
        }

        [HttpGet]
        public virtual IHttpActionResult Query([FromUri] Query q)
        {
            return HandleException(() =>
            {
                var items = Reader.Query(q)
                                  .Select(Mapper.Map)
                                  .ToList();
                var result = Request.EnrichQueryResult(items, q);

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