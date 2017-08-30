using Skeleton.Abstraction;
using Skeleton.Abstraction.Dependency;
using Skeleton.Infrastructure.Dependency;
using Skeleton.Web.Server.Configuration;
using Swashbuckle.Application;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Skeleton.Web.Server
{
    public class OwinBootstrapper : Bootstrapper, IBootstrapper
    {
        private readonly HttpConfiguration _configuration;

        public OwinBootstrapper(HttpConfiguration configuration) : base(DependencyContainer.Instance)
        {
            _configuration = configuration;
        }

        public void Configure()
        {
            _configuration.DependencyResolver = new UnityResolver(DependencyContainer.Instance.UnityContainer);
            _configuration.MapHttpAttributeRoutes();
            _configuration.Routes.MapHttpRoute(
                            name: Constants.Routes.DefaultRpcRoute,
                            routeTemplate: Constants.Routes.DefaultRpcRouteTemplate,
                            defaults: new { id = RouteParameter.Optional });
            _configuration.EnsureInitialized();
        }

        public void UseSwagger()
        {
            _configuration
                .EnableSwagger(c =>
                    {
                    c.SingleApiVersion("v1", "Skeleton Api")
                     .Description("Skeleton API for coding REST operations")
                     .Contact(cc => cc
                            .Name("Jej666")
                            .Email("jej666@gmail.com"));
                    })
                 .EnableSwaggerUi();
        }

        public void UseValidateModel()
        {
            _configuration.Filters.Add(new CheckModelForNullAttribute());
            _configuration.Filters.Add(new ValidateModelStateAttribute());
        }

        public void UseGlobalExceptionHandling()
        {
            var loggerFactory = Container.Resolve<ILoggerFactory>();

            _configuration.Services.Add(typeof(IExceptionLogger), new GlobalExceptionLogger(loggerFactory));
            _configuration.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
        }
    }
}