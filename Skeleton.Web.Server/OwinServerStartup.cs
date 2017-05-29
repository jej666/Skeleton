using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using Skeleton.Abstraction;
using Skeleton.Web.Server;
using Skeleton.Web.Server.Configuration;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(OwinServerStartup))]

namespace Skeleton.Web.Server
{
    public class OwinServerStartup
    {
        private readonly static Lazy<WebAppHost> AppHost = 
            new Lazy<WebAppHost>( ()=> new WebAppHost());

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.RegisterWebApi(AppHost.Value);
            config.RegisterSwagger();

#if DEBUG
            app.UseRequestLogger(config);
#else
            app.UseSsl();
#endif

            app.UseCompression();
            app.UseWebApi(config);
        }

        public static IAppHost WebAppHost => AppHost.Value;

        public static IDisposable StartServer(Uri url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));

            return WebApp.Start<OwinServerStartup>(url.ToString());
        }
    }
}