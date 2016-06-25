using Skeleton.Abstraction;
using Skeleton.Core.Service;
using Skeleton.Web.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Routing;

namespace Skeleton.Web.Server
{
    public class ReadController<TEntity, TIdentity, TDto> :
        ApiController
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly IReadService<TEntity, TIdentity> _service;
        private readonly IEntityMapper<TEntity, TIdentity> _mapper;

        public ReadController(
            IReadService<TEntity, TIdentity> service,
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _service.Dispose();

            base.Dispose(disposing);
        }
    }
}