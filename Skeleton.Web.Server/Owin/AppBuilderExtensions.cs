using Owin;
using Skeleton.Abstraction;
using System.Web.Http;

namespace Skeleton.Web.Server.Owin
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseRequestLogger(this IAppBuilder app, HttpConfiguration config)
        {
            var loggerFactory = config.DependencyResolver.GetService(typeof(ILoggerFactory));

            app.Use<RequestLoggingMiddleware>(loggerFactory);

            return app;
        }

        public static IAppBuilder UseCompression(this IAppBuilder app)
        {
            app.Use<CompressionMiddleware>();

            return app;
        }

        public static IAppBuilder UseSsl(this IAppBuilder app)
        {
            app.Use<RequireSslMiddleware>();

            return app;
        }
    }
}