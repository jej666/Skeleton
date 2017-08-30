using Skeleton.Abstraction;
using Skeleton.Abstraction.Dependency;
using Skeleton.Infrastructure.Dependency;
using Skeleton.Web.Server.Configuration;
using Swashbuckle.Application;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Skeleton.Web.Server
{
    public class OwinBootstrapper : Bootstrapper, IOwinBootstrapper
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
        }

        public IOwinBootstrapper UseSwagger()
        {
            _configuration.EnsureInitialized();
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

            return this;
        }

        public IOwinBootstrapper UseCheckModelForNull()
        {
            _configuration.Filters.Add(new CheckModelForNullAttribute());

            return this;
        }

        public IOwinBootstrapper UseValidateModelState()
        {
            _configuration.Filters.Add(new ValidateModelStateAttribute());

            return this;
        }

        public IOwinBootstrapper UseGlobalExceptionHandling()
        {
            var loggerFactory = Container.Resolve<ILoggerFactory>();

            _configuration.Services.Add(typeof(IExceptionLogger), new GlobalExceptionLogger(loggerFactory));
            _configuration.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

            return this;
        }
    }
}