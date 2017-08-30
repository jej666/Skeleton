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
        private OwinServerOptions _options;

        public IDisposable StartServer(Uri url, Action<IBootstrapper> bootstrap)
        {
            return StartServer(url, bootstrap, new OwinServerOptions());
        }

        public IDisposable StartServer(Uri url, Action<IBootstrapper> bootstrap, OwinServerOptions options)
        {
            url.ThrowIfNull();
            bootstrap.ThrowIfNull();
            options.ThrowIfNull();

            _options = options;
            _bootstrapper.Configure();

            bootstrap(_bootstrapper);

            EnableWebApiOptions();

            return WebApp.Start(url.ToString(), appBuilder =>
            {
                EnableOwinOptions(appBuilder);

                appBuilder.UseWebApi(HttpConfiguration.Value);
            });
        }

        private void EnableOwinOptions(IAppBuilder appBuilder)
        {
            if (_options.EnableRequestId)
                appBuilder.Use<RequestIdMiddleware>();

            if (_options.EnableRequestLogging)
            {
                var loggerFactory = DependencyContainer.Instance.Resolve<ILoggerFactory>();
                appBuilder.Use<RequestLoggerMiddleware>(loggerFactory);
            }

            if (_options.RequireSsl)
                appBuilder.Use<RequireSslMiddleware>();

            if (_options.EnableCompression)
                appBuilder.Use<CompressionMiddleware>();
        }

        private void EnableWebApiOptions()
        {
            if (_options.ValidateModel)
                _bootstrapper.UseValidateModel();

            if (_options.EnableGlobalExceptionHandling)
                _bootstrapper.UseGlobalExceptionHandling();

            if (_options.EnableSwagger)
                _bootstrapper.UseSwagger();
        }
    }
}