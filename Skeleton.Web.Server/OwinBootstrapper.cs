using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Dependency;
using Skeleton.Infrastructure.Dependency;
using Skeleton.Web.Server.Configuration;
using Skeleton.Web.Server.Owin;
using Swashbuckle.Application;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Skeleton.Web.Server
{
    public class OwinBootstrapper : Bootstrapper, IOwinBootstrapper
    {
        private readonly HttpConfiguration _configuration = new HttpConfiguration();

        public OwinBootstrapper() : base(DependencyContainer.Instance)
        {
            _configuration.DependencyResolver = new UnityResolver(DependencyContainer.Instance.UnityContainer);
            _configuration.MapHttpAttributeRoutes();
            _configuration.Routes.MapHttpRoute(
                            name: Constants.DefaultHttpRoute,
                            routeTemplate: Constants.DefaultRouteTemplate,
                            defaults: new { id = RouteParameter.Optional });

            var defaultSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            JsonConvert.DefaultSettings = () => { return defaultSettings; };

            _configuration.Formatters.Clear();
            _configuration.Formatters.Add(new JsonMediaTypeFormatter());
            _configuration.Formatters.JsonFormatter.SerializerSettings = defaultSettings;
        }

        // Runtime call
        public void Configuration(IAppBuilder appBuilder)
        { 
#if DEBUG
            appBuilder.Use<RequestLoggingMiddleware>(Container.Resolve<ILoggerFactory>());
#else
            appBuilder.Use<RequireSslMiddleware>();
#endif
            appBuilder.Use<CompressionMiddleware>();

            _configuration.EnsureInitialized();
            appBuilder.UseWebApi(_configuration);
        }

        public IOwinBootstrapper UseSwagger()
        {
            _configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Skeleton Api")
                     .Description("Skeleton API for coding REST operations")
                     .TermsOfService("NA")
                     .Contact(cc => cc
                            .Name("Jej666")
                            .Email("jej666@gmail.com"));
                    c.IgnoreObsoleteProperties();
                    c.DescribeAllEnumsAsStrings();
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