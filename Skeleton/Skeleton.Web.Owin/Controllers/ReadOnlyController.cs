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
    public class ReadOnlyController<TEntity, TIdentity> :
        ApiController
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IReadOnlyService<TEntity, TIdentity> _service;

        public ReadOnlyController(IReadOnlyService<TEntity, TIdentity> service)
        {
            service.ThrowIfNull(() => service);

            _service = service;
        }

        // GET api/<controller>
        public virtual IEnumerable<TEntity> Get()
        {
            return _service.Repository.GetAll();
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