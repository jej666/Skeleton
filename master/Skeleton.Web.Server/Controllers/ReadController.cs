using System.Linq;
using System.Web.Http;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Web.Server.Controllers
{
    public class ReadController<TEntity, TIdentity, TDto> :
            ApiController
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly IReadRepository<TEntity, TIdentity, TDto> _repository;

        public ReadController(IReadRepository<TEntity, TIdentity, TDto> repository)
        {
            _repository = repository;
        }

        // GET api/<controller>/5
        public virtual IHttpActionResult Get(TIdentity id)
        {
            var result = _repository.Query.FirstOrDefault(id);

            if (result == null)
                return NotFound();

            return Ok(_repository.Mapper.Map(result));
        }

        public virtual IHttpActionResult Get()
        {
            var allData = _repository.Query
                .GetAll()
                .Select(_repository.Mapper.Map)
                .ToList();

            return Ok(allData);
        }

        // GET api/<controller>/?pageSize=20&pageNumber=1
        [HttpGet]
        public virtual IHttpActionResult Page(int pageSize, int pageNumber)
        {
            var totalCount = _repository.Query.Count();
            var pagedData = _repository.Query
                .Page(pageSize, pageNumber)
                .Select(_repository.Mapper.Map)
                .ToList();
            var pagedResult = Request.SetPagedResult(
                totalCount, pageNumber, pageSize, pagedData);

            return Ok(pagedResult);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _repository.Dispose();

            base.Dispose(disposing);
        }
    }
}