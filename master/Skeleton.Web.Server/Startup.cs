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
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.Register();
            app.UseWebApi(config);
        }

        public static IDisposable StartServer(string baseAddress)
        {
            return WebApp.Start<Startup>(baseAddress);
        }
    }
}