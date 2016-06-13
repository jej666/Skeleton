using Skeleton.Abstraction;
using Skeleton.Core.Domain;
using Skeleton.Core.Service;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System;

namespace Skeleton.Web.Server
{
    public class ReadOnlyController<TEntity, TIdentity, TDto> :
        ApiController
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly IReadOnlyService<TEntity, TIdentity> _service;
        private readonly IEntityMapper<TEntity, TIdentity> _mapper;

        public ReadOnlyController(
            IReadOnlyService<TEntity, TIdentity> service, 
            IEntityMapper<TEntity, TIdentity> mapper)
        {
            service.ThrowIfNull(() => service);
            mapper.ThrowIfNull(() => mapper);

            _service = service;
            _mapper = mapper;
        }

        // GET api/<controller>
        public virtual IEnumerable<TDto> Get()
        {
            return _service.Repository
                           .GetAll()
                           .Select(_mapper.Map<TDto>)
                           .ToList();
        }

        //[HttpGet]
        //public virtual IEnumerable<TEntity> Page(int pageSize, int pageNumber)
        //{
        //    return _service.Repository.Page(pageSize, pageNumber);
        //}

        // GET api/<controller>/5
        public virtual TEntity Get(TIdentity id)
        {
            return _service.Repository.FirstOrDefault(id);
        }

        protected IEnumerable GetModelErrors()
        {
            return this.ModelState.SelectMany(
                x => x.Value.Errors.Select(
                    error => error.ErrorMessage));
        }
    }
}