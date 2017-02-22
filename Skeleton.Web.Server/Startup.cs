using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using Skeleton.Abstraction;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Infrastructure.Logging;
using Skeleton.Web.Server;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(Startup))]

namespace Skeleton.Web.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.RegisterWebApi();
            config.RegisterSwagger();

            var loggerFactory = config.DependencyResolver.GetService(typeof(ILoggerFactory));
            app.Use<RequestLoggingMiddleware>(loggerFactory);
            app.UseWebApi(config);
        }

        public static IDisposable StartServer(string baseAddress)
        {
            return WebApp.Start<Startup>(baseAddress);
        }
    }
}