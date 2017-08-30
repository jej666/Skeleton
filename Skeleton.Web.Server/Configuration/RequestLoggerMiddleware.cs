using Microsoft.Owin;
using Skeleton.Abstraction;
using Skeleton.Core;
using Skeleton.Web.Server.Helpers;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Skeleton.Web.Server.Configuration
{
    public sealed class RequestLoggerMiddleware : OwinMiddleware
    {
        private readonly ILogger _log;

        public RequestLoggerMiddleware(OwinMiddleware next, ILoggerFactory loggerFactory)
            : base(next)
        {
            loggerFactory.ThrowIfNull();

            _log = loggerFactory.GetLogger(this.GetType());
        }

        public override async Task Invoke(IOwinContext context)
        {
            await LogRequest(context).ConfigureAwait(false);
        }

        // https://blog.heroku.com/http_request_id_s_improve_visibility_across_the_application_stack
        private async Task LogRequest(IOwinContext context)
        {
            var requestId = context.GetRequestId();
            
            var stopWath = Stopwatch.StartNew();
            await Next.Invoke(context).ConfigureAwait(false);
            stopWath.Stop();
           
            _log.Info($"RequestId: {requestId} - Method :{context.Request.Method} - Url: {context.Request.Uri.PathAndQuery} - TimeElapsed: {stopWath.ElapsedMilliseconds} ms");
        } 
    }
}