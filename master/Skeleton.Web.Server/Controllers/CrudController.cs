using System.Collections.Generic;
using System.Web.Http;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Web.Server.Controllers
{
    public class CrudController<TEntity, TDto> :
            ReadController<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        private readonly ICrudRepository<TEntity, TDto> _repository;

        public CrudController(
            ICrudRepository<TEntity, TDto> repository)
            : base(repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IHttpActionResult Update(TDto dto)
        {
            var entity = _repository.Mapper.Map(dto);
            var result = _repository.Store.Update(entity);

            if (result)
                return Ok();

            return NotFound();
        }

        [HttpPost]
        public IHttpActionResult UpdateMany(IEnumerable<TDto> dtos)
        {
            var entities = _repository.Mapper.Map(dtos);
            var result = _repository.Store.Update(entities);

            if (result)
                return Ok();

            return NotFound();
        }

        [HttpPost]
        public IHttpActionResult Add(TDto dto)
        {
            var entity = _repository.Mapper.Map(dto);
            var result = _repository.Store.Add(entity);

            if (!result)
                return NotFound();

            var newDto = _repository.Mapper.Map(entity);

            return CreatedAtRoute(
                "DefaultApiWithId",
                new { id = entity.Id },
                newDto);
        }

        [HttpPost]
        public IHttpActionResult AddMany(IEnumerable<TDto> dtos)
        {
            var entities = _repository.Mapper.Map(dtos);
            var result = _repository.Store.Add(entities);

            if (result)
                return Ok(_repository.Mapper.Map(entities));

            return NotFound();
        }

        [HttpGet]
        public IHttpActionResult Delete(string id)
        {
            var entity = _repository.Query.FirstOrDefault(id);

            if (entity == null)
                return NotFound();

            var result = _repository.Store.Delete(entity);

            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        public IHttpActionResult DeleteMany(IEnumerable<TDto> dtos)
        {
            var entities = _repository.Mapper.Map(dtos);
            var result = _repository.Store.Delete(entities);

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