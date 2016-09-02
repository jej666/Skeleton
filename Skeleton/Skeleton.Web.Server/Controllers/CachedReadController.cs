using System.Linq;
using System.Web.Http;
using Skeleton.Core;
using Skeleton.Core.Repository;

namespace Skeleton.Web.Server.Controllers
{
    public class CachedReadController<TEntity, TIdentity, TDto> :
            ApiController
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly ICachedReadService<TEntity, TIdentity, TDto> _service;

        public CachedReadController(
            ICachedReadService<TEntity, TIdentity, TDto> service)
        {
            _service = service;
        }

        // GET api/<controller>/5
        public virtual IHttpActionResult Get(TIdentity id)
        {
            var result = _service.Query.FirstOrDefault(id);

            if (result == null)
                return NotFound();

            return Ok(_service.Mapper.Map(result));
        }

        public virtual IHttpActionResult Get()
        {
            var allData = _service.Query
                .GetAll()
                .Select(_service.Mapper.Map)
                .ToList();

            return Ok(allData);
        }

        // GET api/<controller>/?pageSize=20&pageNumber=1
        [HttpGet]
        public virtual IHttpActionResult Page(int pageSize, int pageNumber)
        {
            var totalCount = _service.Query.Count();
            var pagedData = _service.Query
                .Page(pageSize, pageNumber)
                .Select(_service.Mapper.Map)
                .ToList();
            var pagedResult = Request.SetPagedResult(
                totalCount, pageNumber, pageSize, pagedData);

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