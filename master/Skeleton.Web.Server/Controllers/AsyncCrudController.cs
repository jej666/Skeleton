using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Web.Server.Controllers
{
    public class AsyncCrudController<TEntity, TIdentity, TDto> :
            AsyncReadController<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly IAsyncCrudRepository<TEntity, TIdentity, TDto> _repository;

        public AsyncCrudController(
            IAsyncCrudRepository<TEntity, TIdentity, TDto> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public async Task<IHttpActionResult> Put(TIdentity id, TDto dto)
        {
            var entity = _repository.Mapper.Map(id, dto);
            var result = await _repository.Store.UpdateAsync(entity);

            if (result)
                return Ok();

            return NotFound();
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateMany(IEnumerable<TDto> dtos)
        {
            var entities = _repository.Mapper.Map(dtos);
            var result = await _repository.Store.UpdateAsync(entities);

            if (result)
                return Ok();

            return NotFound();
        }

        public async Task<IHttpActionResult> Post(TDto dto)
        {
            var entity = _repository.Mapper.Map(dto);
            var result = await _repository.Store.AddAsync(entity);

            if (!result)
                return NotFound();

            var newDto = _repository.Mapper.Map(entity);

            return CreatedAtRoute(
                "DefaultApiWithId",
                new {id = entity.Id},
                newDto);
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddMany(IEnumerable<TDto> dtos)
        {
            var entities = _repository.Mapper.Map(dtos);
            var result = await _repository.Store.AddAsync(entities);

            if (result)
                return Ok(_repository.Mapper.Map(entities));

            return NotFound();
        }

        public async Task<IHttpActionResult> Delete(TIdentity id)
        {
            var entity = await _repository.Query.FirstOrDefaultAsync(id);

            if (entity == null)
                return NotFound();

            var result = await _repository.Store.DeleteAsync(entity);

            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        public async Task<IHttpActionResult> DeleteMany(IEnumerable<TDto> dtos)
        {
            var entities = _repository.Mapper.Map(dtos);
            var result = await _repository.Store.DeleteAsync(entities);

            if (result)
                return Ok();

            return BadRequest();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _repository.Dispose();

            base.Dispose(disposing);
        }
    }
}