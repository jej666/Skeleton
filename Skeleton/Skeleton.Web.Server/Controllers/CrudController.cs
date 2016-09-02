using System.Collections.Generic;
using System.Web.Http;
using Skeleton.Core;
using Skeleton.Core.Repository;

namespace Skeleton.Web.Server.Controllers
{
    public class CrudController<TEntity, TIdentity, TDto> :
            ReadController<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly ICrudService<TEntity, TIdentity, TDto> _service;

        public CrudController(
            ICrudService<TEntity, TIdentity, TDto> service)
            : base(service)
        {
            _service = service;
        }

        public IHttpActionResult Put(TIdentity id, TDto dto)
        {
            var entity = _service.Mapper.Map(id, dto);
            var result = _service.Store.Update(entity);

            if (result)
                return Ok();

            return NotFound();
        }

        [HttpPost]
        public IHttpActionResult UpdateMany(IEnumerable<TDto> dtos)
        {
            var entities = _service.Mapper.Map(dtos);
            var result = _service.Store.Update(entities);

            if (result)
                return Ok();

            return NotFound();
        }

        public IHttpActionResult Post(TDto dto)
        {
            var entity = _service.Mapper.Map(dto);
            var result = _service.Store.Add(entity);

            if (!result)
                return NotFound();

            var newDto = _service.Mapper.Map(entity);

            return CreatedAtRoute(
                "DefaultApiWithId",
                new {id = entity.Id},
                newDto);
        }

        [HttpPost]
        public IHttpActionResult AddMany(IEnumerable<TDto> dtos)
        {
            var entities = _service.Mapper.Map(dtos);
            var result = _service.Store.Add(entities);

            if (result)
                return Ok(_service.Mapper.Map(entities));

            return NotFound();
        }

        public IHttpActionResult Delete(TIdentity id)
        {
            var entity = _service.Query.FirstOrDefault(id);

            if (entity == null)
                return NotFound();

            var result = _service.Store.Delete(entity);

            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        public IHttpActionResult DeleteMany(IEnumerable<TDto> dtos)
        {
            var entities = _service.Mapper.Map(dtos);
            var result = _service.Store.Delete(entities);

            if (result)
                return Ok();

            return BadRequest();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _service.Dispose();

            base.Dispose(disposing);
        }
    }
}