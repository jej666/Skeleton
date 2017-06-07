using Microsoft.Owin;
using Skeleton.Abstraction;
using Skeleton.Common;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Skeleton.Web.Server.Owin
{
    public sealed class RequestLoggingMiddleware : OwinMiddleware
    {
        private readonly ILogger _log;

        public RequestLoggingMiddleware(OwinMiddleware next, ILoggerFactory loggerFactory)
            : base(next)
        {
            loggerFactory.ThrowIfNull(nameof(loggerFactory));

            _log = loggerFactory.GetLogger(this.GetType());
        }

        public override async Task Invoke(IOwinContext context)
        {
            await MeasureRequestExecutionTime(context).ConfigureAwait(false);
        }

        private async Task MeasureRequestExecutionTime(IOwinContext context)
        {
            var stopWath = Stopwatch.StartNew();
            await Next.Invoke(context).ConfigureAwait(false);
            stopWath.Stop();

            _log.Info($"Request {context.Request.Uri.PathAndQuery} took {stopWath.ElapsedMilliseconds} ms");
        }
    }
}