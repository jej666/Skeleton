using Owin;
using Skeleton.Abstraction;
using Skeleton.Common;
using System.Web.Http;

namespace Skeleton.Web.Server
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseRequestLogger(this IAppBuilder app, HttpConfiguration config)
        {
            app.ThrowIfNull(() => app);
            config.ThrowIfNull(() => config);

            var loggerFactory = config.DependencyResolver.GetService(typeof(ILoggerFactory));

            app.Use<RequestLoggingMiddleware>(loggerFactory);

            return app;
        }
    }
}