using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using Skeleton.Web.Server;
using Skeleton.Web.Server.Configuration;
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

#if DEBUG
            app.UseErrorPage();
#endif
            app.UseCompression();
            app.UseRequestLogger(config);
            app.UseWebApi(config);
        }

        public static IDisposable StartServer(Uri url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));

            return WebApp.Start<Startup>(url.ToString());
        }
    }
}