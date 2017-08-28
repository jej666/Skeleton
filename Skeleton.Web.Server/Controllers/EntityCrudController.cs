using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;
using System.Collections.Generic;
using System.Web.Http;

namespace Skeleton.Web.Server.Controllers
{
    public class EntityCrudController<TEntity, TDto> :
        EntityReaderController<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>, new()
        where TDto : class, new()
    {
        private readonly IEntityWriter<TEntity> _writer;

        public EntityCrudController(
            ILogger logger,
            IEntityReader<TEntity> reader,
            IEntityMapper<TEntity, TDto> mapper,
            IEntityWriter<TEntity> writer) :
            base(logger, reader, mapper)
        {
            _writer = writer;
        }

        public IEntityWriter<TEntity> Writer => _writer;

        [HttpPut]
        public virtual IHttpActionResult Update(string id, [FromBody]TDto dto)
        {
            return HandleException(() =>
            {
                var entity = Reader.FirstOrDefault(id);

                if (entity == null)
                    return NotFound();

                var updateableEntity = Mapper.Map(dto);
                var result = Writer.Update(updateableEntity);

                if (result)
                    return Ok();

                return NotFound();
            });
        }

        [HttpPost]
        public virtual IHttpActionResult BatchUpdate(IEnumerable<TDto> dtos)
        {
            return HandleException(() =>
            {
                var entities = Mapper.Map(dtos);
                var result = Writer.Update(entities);

                if (result)
                    return Ok();

                return NotFound();
            });
        }

        [HttpPost]
        public virtual IHttpActionResult Create(TDto dto)
        {
            return HandleException(() =>
            {
                var entity = Mapper.Map(dto);
                var result = Writer.Add(entity);

                if (!result)
                    return Conflict();

                var newDto = Mapper.Map(entity);

                return CreatedAtRoute(
                    Constants.DefaultRpcRoute,
                    new { id = entity.Id },
                    newDto);
            });
        }

        [HttpPost]
        public virtual IHttpActionResult BatchCreate(IEnumerable<TDto> dtos)
        {
            return HandleException(() =>
            {
                var entities = Mapper.Map(dtos);
                var result = Writer.Add(entities);

                if (result)
                    return Ok(Mapper.Map(entities));

                return Conflict();
            });
        }

        [HttpDelete]
        public virtual IHttpActionResult Delete(string id)
        {
            return HandleException(() =>
            {
                var entity = Reader.FirstOrDefault(id);

                if (entity == null)
                    return NotFound();

                var result = Writer.Delete(entity);

                if (result)
                    return Ok();

                return BadRequest();
            });
        }

        [HttpPost]
        public virtual IHttpActionResult BatchDelete(IEnumerable<TDto> dtos)
        {
            return HandleException(() =>
            {
                var entities = Mapper.Map(dtos);
                var result = Writer.Delete(entities);

                if (result)
                    return Ok();

                return BadRequest();
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _writer.Dispose();

            base.Dispose(disposing);
        }
    }
}