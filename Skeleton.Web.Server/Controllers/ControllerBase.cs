using Skeleton.Abstraction;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Skeleton.Web.Server.Controllers
{
    public abstract class ControllerBase : ApiController
    {
        private readonly ILogger _logger;

        protected ControllerBase(ILogger logger)
        {
            _logger = logger;
        }

        public ILogger Logger => _logger;

        protected virtual IHttpActionResult HandleException(Func<IHttpActionResult> handler)
        {
            try
            {
                return handler();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest();
            }
        }

        protected virtual async Task<IHttpActionResult> HandleExceptionAsync(Func<Task<IHttpActionResult>> handler)
        {
            try
            {
                return await handler();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest();
            }
        }
    }
}