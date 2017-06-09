using Skeleton.Infrastructure.Dependency;
using Skeleton.Web.Server.Configuration;
using System;
using System.Web.Http;

namespace Skeleton.Web.Server
{
    public class OwinBootstrapper : Bootstrapper
    {
        private static readonly Lazy<HttpConfiguration> Configuration =
            new Lazy<HttpConfiguration>(() => new HttpConfiguration());

        public HttpConfiguration HttpConfiguration => Configuration.Value;

        public OwinBootstrapper() : base(DependencyContainer.Instance)
        {
        }

        public void ConfigureMinimalWebApi()
        {
            HttpConfiguration.DependencyResolver = new UnityResolver(DependencyContainer.Instance.UnityContainer);
            HttpConfiguration.MapHttpAttributeRoutes();
            HttpConfiguration.Routes.MapHttpRoute(
                            name: Constants.DefaultHttpRoute,
                            routeTemplate: Constants.DefaultRouteTemplate,
                            defaults: new { id = RouteParameter.Optional });
        }

        public void ConfigureCommmonWebApiFeatures()
        {
            HttpConfiguration.UseJson()
                             .UseSwagger()
                             .UseCheckModelForNull()
                             .UseValidateModelState()
                             .UseGlobalExceptionHandling();
        }
    }
}