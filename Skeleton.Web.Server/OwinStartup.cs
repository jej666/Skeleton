using Microsoft.Owin.Hosting;
using Owin;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Dependency;
using Skeleton.Core;
using Skeleton.Infrastructure.Dependency;
using Skeleton.Web.Server.Configuration;
using System;
using System.Web.Http;

namespace Skeleton.Web.Server
{
    public class OwinStartup
    {
        private static readonly Lazy<HttpConfiguration> HttpConfiguration =
            new Lazy<HttpConfiguration>(() => new HttpConfiguration());

        private readonly OwinBootstrapper _bootstrapper =
            new OwinBootstrapper(HttpConfiguration.Value);
        private OwinServerOptions _options = new OwinServerOptions
        {
            EnableCompression = true,
            EnableRequestId = true,
            EnableRequestLogging = true,
            RequireSsl = false
        };

        public IDisposable StartServer(Uri url, Action<IOwinBootstrapper> bootstrap)
        {
            return StartServer(url, bootstrap, _options);
        }

        public IDisposable StartServer(Uri url, Action<IOwinBootstrapper> bootstrap, OwinServerOptions options)
        {
            url.ThrowIfNull();
            bootstrap.ThrowIfNull();
            options.ThrowIfNull();

            _options = options;
            _bootstrapper.Configure();

            bootstrap(_bootstrapper);

            return WebApp.Start<OwinStartup>(url.ToString());
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            if (_options.EnableRequestId)
                appBuilder.Use<RequestIdMiddleware>();

            if (_options.EnableRequestLogging)
                appBuilder.Use<RequestLoggerMiddleware>(DependencyContainer.Instance.Resolve<ILoggerFactory>());

            if (_options.RequireSsl)
                appBuilder.Use<RequireSslMiddleware>();

            if (_options.EnableCompression)
                appBuilder.Use<CompressionMiddleware>();

            appBuilder.UseWebApi(HttpConfiguration.Value);
        }
    }
}