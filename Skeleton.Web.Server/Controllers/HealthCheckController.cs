using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Skeleton.Web.Server.Controllers
{
    public class HealthCheckController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> HeartBeat()
        {
            return await Task.FromResult(Ok());
        }

        [HttpGet]
        public async Task<IHttpActionResult> ServerInfo()
        {
            return await Task.FromResult(Ok(new { DateTime.Now }));
        }
    }
}