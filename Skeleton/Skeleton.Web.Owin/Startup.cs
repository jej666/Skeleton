using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using Skeleton.Infrastructure.DependencyResolver;
using System;
using System.Net.Http.Headers;
using System.Web.Http;

[assembly: OwinStartup(typeof(Skeleton.Web.Server.Startup))]

namespace Skeleton.Web.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));

            config.DependencyResolver = new UnityResolver(Bootstrapper.Container);

            app.UseWebApi(config);
        }

        public static void StartConsoleServer()
        {
            string baseAddress = "http://localhost:8081/";

            using(WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("Started...");
                Console.ReadKey();
            }
        }

        public static IDisposable StartServer(string baseAddress)
        {
            return WebApp.Start<Startup>(baseAddress);
        }
    }
}
