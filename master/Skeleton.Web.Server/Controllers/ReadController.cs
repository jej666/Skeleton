using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Http;

namespace Skeleton.Web.Server.Controllers
{
    public class ReadController<TEntity, TDto> :
            ApiController
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        private readonly IReadRepository<TEntity, TDto> _repository;

        public ReadController(IReadRepository<TEntity, TDto> repository)
        {
            repository.ThrowIfNull(() => repository);

            _repository = repository;
        }

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
        [HttpGet]
        public virtual IHttpActionResult Get(string id)
        {
            var result = _repository.Query.FirstOrDefault(id);

            if (result == null)
                return NotFound();

            return Ok(_repository.Mapper.Map(result));
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [HttpGet]
        public virtual IHttpActionResult GetAll()
        {
            var allData = _repository.Query
                .Find()
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
            var pagedResult = Request.ToPagedResult(
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