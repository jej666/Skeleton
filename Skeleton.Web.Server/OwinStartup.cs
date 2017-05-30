using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using Skeleton.Abstraction;
using Skeleton.Web.Server;
using Skeleton.Web.Server.Configuration;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(OwinStartup))]

namespace Skeleton.Web.Server
{
    public class OwinStartup
    {
        private readonly static Lazy<OwinBootstrapper> OwinBootstrapper = 
            new Lazy<OwinBootstrapper>( ()=> new OwinBootstrapper());

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.RegisterWebApi(OwinBootstrapper.Value);
            config.RegisterSwagger();

#if DEBUG
            app.UseRequestLogger(config);
#else
            app.UseSsl();
#endif

            app.UseCompression();
            app.UseWebApi(config);
        }

        public static IBootstrapper Bootstrapper => OwinBootstrapper.Value;

        public static IDisposable StartServer(Uri url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));

            return WebApp.Start<OwinStartup>(url.ToString());
        }
    }
}