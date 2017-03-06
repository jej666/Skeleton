using Microsoft.Owin;
using Skeleton.Abstraction;
using Skeleton.Common;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Skeleton.Web.Server.Middlewares
{
    public sealed class RequestLoggingMiddleware : OwinMiddleware
    {
        private readonly ILogger _log;

        public RequestLoggingMiddleware(OwinMiddleware next, ILoggerFactory loggerFactory)
            : base(next)
        {
            loggerFactory.ThrowIfNull(() => loggerFactory);

            _log = loggerFactory.GetLogger(this.GetType());
        }

        public override async Task Invoke(IOwinContext context)
        {
            await Measure(context).ConfigureAwait(false);
        }

        private async Task Measure(IOwinContext context)
        {
            var stopWath = Stopwatch.StartNew();
            await Next.Invoke(context).ConfigureAwait(false);
            stopWath.Stop();

            _log.Info($"Request {context.Request.Uri.PathAndQuery} took {stopWath.ElapsedMilliseconds} ms");
        }
    }
}