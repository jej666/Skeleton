using Skeleton.Abstraction;
using Skeleton.Web.Server.Helpers;
using System.Web.Http;

namespace Skeleton.Web.Server.Controllers
{
    public sealed class LogController : ControllerBase
    {
        public LogController(ILogger logger) :
            base(logger)
        {
        }

        [HttpPost]
        public IHttpActionResult Info([FromBody] string message)
        {
            var ip = Request.GetClientIp();
            Logger.Info($"Client IP :: {ip} - Message :: {message}");

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Debug([FromBody] string message)
        {
            var ip = Request.GetClientIp();
            Logger.Debug($"Client IP :: {ip} - Message :: {message}");

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Error([FromBody] string message)
        {
            var ip = Request.GetClientIp();
            Logger.Error($"Client IP :: {ip} - Message :: {message}");

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Warn([FromBody] string message)
        {
            var ip = Request.GetClientIp();
            Logger.Warn($"Client IP :: {ip} - Message :: {message}");

            return Ok();
        }
    }
}