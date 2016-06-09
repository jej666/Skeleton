using System;
using System.Threading.Tasks;
using Owin;
using System.Web.Http;
using Microsoft.Owin;
using System.Net.Http.Headers;
using Skeleton.Infrastructure.DependencyResolver;
using Microsoft.Owin.Hosting;

[assembly: OwinStartup(typeof(Skeleton.Web.Owin.Startup))]

namespace Skeleton.Web.Owin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            config.Formatters.JsonFormatter.
               SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.DependencyResolver = new UnityResolver(Bootstrapper.Container);

            app.UseWebApi(config);
        }

        public static void StartServer()
        {
            string baseAddress = "http://localhost:8081/";

            IDisposable webApplication = WebApp.Start<Startup>(baseAddress);

            try
            {
                Console.WriteLine("Started...");

                Console.ReadKey();
            }
            finally
            {
                webApplication.Dispose();
            }
        }
    }
}
