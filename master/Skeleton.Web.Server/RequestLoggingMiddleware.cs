using Microsoft.Owin;
using Skeleton.Abstraction;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Infrastructure.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Skeleton.Web.Server
{
    public sealed class RequestLoggingMiddleware : OwinMiddleware
    {
        private readonly ILogger _log;

        public RequestLoggingMiddleware(OwinMiddleware next, ILoggerFactory loggerFactory) 
            : base(next)
        {
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
