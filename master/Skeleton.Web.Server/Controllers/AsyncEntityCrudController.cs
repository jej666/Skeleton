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
            IAsyncEntityReader<TEntity> reader,
            IAsyncEntityWriter<TEntity> writer,
            IEntityMapper<TEntity, TDto> mapper) :
            base(reader, mapper)
        {
            _writer = writer;
        }

        public IAsyncEntityWriter<TEntity> Writer => _writer;

        [HttpPut]
        public async Task<IHttpActionResult> Update(TDto dto)
        {
            var entity = Mapper.Map(dto);
            var result = await Writer.UpdateAsync(entity);

            if (result)
                return Ok();

            return NotFound();
        }

        [HttpPost]
        public async Task<IHttpActionResult> BatchUpdate(IEnumerable<TDto> dtos)
        {
            var entities = Mapper.Map(dtos);
            var result = await Writer.UpdateAsync(entities);

            if (result)
                return Ok();

            return NotFound();
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create(TDto dto)
        {
            var entity = Mapper.Map(dto);
            var result = await Writer.AddAsync(entity);

            if (!result)
                return NotFound();

            var newDto = Mapper.Map(entity);

            return CreatedAtRoute(
                Constants.DefaultHttpRoute,
                new { id = entity.Id },
                newDto);
        }

        [HttpPost]
        public async Task<IHttpActionResult> BatchCreate(IEnumerable<TDto> dtos)
        {
            var entities = Mapper.Map(dtos);
            var result = await Writer.AddAsync(entities);

            if (result)
                return Ok(Mapper.Map(entities));

            return NotFound();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var entity = await Reader.FirstOrDefaultAsync(id);

            if (entity == null)
                return NotFound();

            var result = await Writer.DeleteAsync(entity);

            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        public async Task<IHttpActionResult> BatchDelete(IEnumerable<TDto> dtos)
        {
            var entities = Mapper.Map(dtos);
            var result = await Writer.DeleteAsync(entities);

            if (result)
                return Ok();

            return BadRequest();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _writer.Dispose();

            base.Dispose(disposing);
        }
    }
}