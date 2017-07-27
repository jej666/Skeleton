using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Skeleton.Web.Server.Controllers
{
    public class AsyncEntityCrudController<TEntity, TDto> :
        AsyncEntityReaderController<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        private readonly IAsyncEntityWriter<TEntity> _writer;

        public AsyncEntityCrudController(
            ILogger logger,
            IAsyncEntityReader<TEntity> reader,
            IEntityMapper<TEntity, TDto> mapper,
            IAsyncEntityWriter<TEntity> writer) :
            base(logger, reader, mapper)
        {
            _writer = writer;
        }

        public IAsyncEntityWriter<TEntity> Writer => _writer;

        [HttpPut]
        public virtual async Task<IHttpActionResult> Update(TDto dto)
        {
            return await HandleExceptionAsync(async () =>
            {
                var entity = Mapper.Map(dto);
                var result = await Writer.UpdateAsync(entity);

                if (result)
                    return Ok();

                return NotFound();
            });
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> BatchUpdate(IEnumerable<TDto> dtos)
        {
            return await HandleExceptionAsync(async () =>
            {
                var entities = Mapper.Map(dtos);
                var result = await Writer.UpdateAsync(entities);

                if (result)
                    return Ok();

                return NotFound();
            });
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> Create(TDto dto)
        {
            return await HandleExceptionAsync(async () =>
            {
                var entity = Mapper.Map(dto);
                var result = await Writer.AddAsync(entity);

                if (!result)
                    return Conflict();

                var newDto = Mapper.Map(entity);

                return CreatedAtRoute(
                    Constants.DefaultHttpRoute,
                    new { id = entity.Id },
                    newDto);
            });
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> BatchCreate(IEnumerable<TDto> dtos)
        {
            return await HandleExceptionAsync(async () =>
            {
                var entities = Mapper.Map(dtos);
                var result = await Writer.AddAsync(entities);

                if (result)
                    return Ok(Mapper.Map(entities));

                return Conflict();
            });
        }

        [HttpDelete]
        public virtual async Task<IHttpActionResult> Delete(string id)
        {
            return await HandleExceptionAsync(async () =>
            {
                var entity = await Reader.FirstOrDefaultAsync(id);

                if (entity == null)
                    return NotFound();

                var result = await Writer.DeleteAsync(entity);

                if (result)
                    return Ok();

                return BadRequest();
            });
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> BatchDelete(IEnumerable<TDto> dtos)
        {
            return await HandleExceptionAsync(async () =>
            {
                var entities = Mapper.Map(dtos);
                var result = await Writer.DeleteAsync(entities);

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