using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using Skeleton.Abstraction.Dependency;
using Skeleton.Web.Server;
using Skeleton.Web.Server.Owin;
using System;

[assembly: OwinStartup(typeof(OwinStartup))]

namespace Skeleton.Web.Server
{
    public class OwinStartup
    {
        private readonly static Lazy<OwinBootstrapper> OwinBootstrapper =
            new Lazy<OwinBootstrapper>(() => new OwinBootstrapper());

        public void Configuration(IAppBuilder app)
        {
            OwinBootstrapper.Value.ConfigureMinimalWebApi();

#if DEBUG
            OwinBootstrapper.Value.ConfigureCommmonWebApiFeatures();
            app.UseRequestLogger(OwinBootstrapper.Value.HttpConfiguration);
#else
            app.UseSsl();
#endif

            app.UseCompression();
            app.UseWebApi(OwinBootstrapper.Value.HttpConfiguration);
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