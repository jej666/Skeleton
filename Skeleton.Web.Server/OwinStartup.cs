using Microsoft.Owin.Hosting;
using Owin;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Dependency;
using Skeleton.Common;
using Skeleton.Infrastructure.Dependency;
using Skeleton.Web.Server.Configuration;
using System;
using System.Web.Http;

namespace Skeleton.Web.Server
{
    public class OwinStartup
    {
        private static readonly HttpConfiguration HttpConfiguration = new HttpConfiguration();
        private readonly OwinBootstrapper _bootstrapper = new OwinBootstrapper(HttpConfiguration);

        public IDisposable StartServer(Uri url, Action<IOwinBootstrapper> bootstrap)
        {
            url.ThrowIfNull(nameof(url));
            bootstrap.ThrowIfNull(nameof(bootstrap));

            _bootstrapper.Configure();
            bootstrap(_bootstrapper);

            return WebApp.Start<OwinStartup>(url.ToString());
        }

        public void Configuration(IAppBuilder appBuilder)
        {
#if DEBUG
            appBuilder.Use<RequestLoggingMiddleware>(DependencyContainer.Instance.Resolve<ILoggerFactory>());
#else
            appBuilder.Use<RequireSslMiddleware>();
#endif
            appBuilder.Use<CompressionMiddleware>()
                      .UseWebApi(HttpConfiguration);
        }
    }
}