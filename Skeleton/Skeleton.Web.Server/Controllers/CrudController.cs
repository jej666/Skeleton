using System;
using System.Linq;
using System.Web.Http;
using Skeleton.Core.Service;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Web.Server
{
    public class CrudController<TEntity, TIdentity, TDto> :
        ApiController
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly IEntityMapper<TEntity, TIdentity> _mapper;
        private readonly ICrudService<TEntity, TIdentity> _service;

        public CrudController(
            ICrudService<TEntity, TIdentity> service,
            IEntityMapper<TEntity, TIdentity> mapper)
        {
            service.ThrowIfNull(() => service);
            mapper.ThrowIfNull(() => mapper);

            _service = service;
            _mapper = mapper;
        }

        // GET api/<controller>/5
        public virtual IHttpActionResult Get(TIdentity id)
        {
            var result = _service.Repository.FirstOrDefault(id);

            if (result == null)
                return NotFound();

            return Ok(_mapper.Map<TDto>(result));
        }

        // GET api/<controller>/
        // GET api/<controller>/?pageSize=20&pageNumber=1
        public virtual IHttpActionResult Get(int? pageSize = null, int? pageNumber = null)
        {
            if (!pageSize.HasValue && !pageNumber.HasValue)
            {
                var allData = _service.Repository
                    .GetAll()
                    .Select(_mapper.Map<TDto>)
                    .ToList();
                return Ok(allData);
            }

            var totalCount = _service.Repository.Count();
            var pagedData = _service.Repository
                .Page(pageSize.Value, pageNumber.Value)
                .Select(_mapper.Map<TDto>)
                .ToList();
            var pagedResult = Request.SetPagedResult(totalCount, pageNumber.Value, pageSize.Value, pagedData);

            return Ok(pagedResult);
        }

        public IHttpActionResult Put(int id, TDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Reverse(id, dto);
            var result = _service.Repository.Update(entity);

            if (result)
                return Ok();

            return NotFound();
        }

        //public IHttpActionResult Post(IEnumerable<TDto> dtos)
        //{
        //    dtos.ThrowIfNullOrEmpty(() => dtos);

        //    var enumerable= dtos.AsList();
        //    foreach (var dto in enumerable)
        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);

        //    var entities = enumerable.Select(_mapper.Reverse);
        //    var result = _service.Repository.Add(entities);

        //    if (result)
        //        return Ok(entities.Select(_mapper.Map<TDto>));

        //    return NotFound();
        //}

        public IHttpActionResult Post(TDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Reverse(dto);
            var result = _service.Repository.Add(entity);

            if (result)
            {
                var newDto = _mapper.Map<TDto>(entity);
                return CreatedAtRoute("DefaultApi", new {id = entity.Id}, newDto);
            }

            return NotFound();
        }

        public IHttpActionResult Delete(TIdentity id)
        {
            var entity = _service.Repository.FirstOrDefault(id);

            if (entity == null)
                return NotFound();

            var result = _service.Repository.Delete(entity);

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