using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using Skeleton.Web.Server;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(Startup))]

namespace Skeleton.Web.Server
{
    public class Startup
    {
        private const string Started = "Started...";

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }

        public static void StartConsoleServer(string baseAddress)
        {
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine(Started);
                Console.ReadKey();
            }
        }

        public static IDisposable StartServer(string baseAddress)
        {
            return WebApp.Start<Startup>(baseAddress);
        }
    }
}