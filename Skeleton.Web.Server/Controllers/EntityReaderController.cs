using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;
using Skeleton.Common;
using Skeleton.Web.Server.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System;

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
        public virtual IHttpActionResult Query([FromUri] Query query)
        {
            return HandleException(() =>
            {
                var data = Reader
                    .Query(query);
                    
                 var dtoData= data.Select(Mapper.Map)
                    .ToList();
                //var pagedResult = Request.ToPagedResult(
                //    totalCount, query.pageNumber, pageSize, data);

                return Ok(dtoData);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _reader.Dispose();

            base.Dispose(disposing);
        }
    }
}