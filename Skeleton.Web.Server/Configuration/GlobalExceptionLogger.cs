using Skeleton.Abstraction;
using Skeleton.Core;
using System.Web.Http.ExceptionHandling;

namespace Skeleton.Web.Server.Configuration
{
    public class GlobalExceptionLogger : ExceptionLogger
    {
        private readonly ILogger _log;

        public GlobalExceptionLogger(ILoggerFactory loggerFactory)
        {
            loggerFactory.ThrowIfNull();

            _log = loggerFactory.GetLogger(this.GetType());
        }

        public override void Log(ExceptionLoggerContext context)
        {
            context.ThrowIfNull();

            _log.Error("API internal error", context.Exception);
        }
    }
}