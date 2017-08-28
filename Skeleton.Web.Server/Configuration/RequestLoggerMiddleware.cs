using Microsoft.Owin;
using Skeleton.Abstraction;
using Skeleton.Core;
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

        private async Task LogRequest(IOwinContext context)
        {
            var stopWath = Stopwatch.StartNew();

            await Next.Invoke(context).ConfigureAwait(false);

            stopWath.Stop();

            _log.Info($"RequestId: {GetRequestId(context)} - Method :{context.Request.Method} - Url: {context.Request.Uri.PathAndQuery} - TimeElapsed: {stopWath.ElapsedMilliseconds} ms");
        }

        private string GetRequestId(IOwinContext context)
        {
            object id;
            context.Environment.TryGetValue("owin.RequestId", out id);

            return id.ToString();
        }
    }
}