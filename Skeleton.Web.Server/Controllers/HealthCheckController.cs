using System.Threading.Tasks;
using System.Web.Http;

namespace Skeleton.Web.Server.Controllers
{
    [Route("api/HealthCheck")]
    public class HealthCheckController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            return await Task.FromResult(Ok());
        }
    }
}