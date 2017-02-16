using Skeleton.Common;
using Skeleton.Infrastructure.DependencyInjection;
using Swashbuckle.Application;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Skeleton.Web.Server
{
    public static class WebApiConfig
    {
        public static void Register(this HttpConfiguration config)
        {
            config.ThrowIfNull(() => config);

            config.RegisterDependencies();
            config.RegisterFormatters();
            config.RegisterFilters();
            config.RegisterRoutes();
            config.EnsureInitialized();
            config.EnableSwagger();
        }

        private static void RegisterFilters(this HttpConfiguration config)
        {
            config.Filters.Add(new CheckModelForNullAttribute());
            config.Filters.Add(new ValidateModelStateAttribute());
        }

        private static void RegisterRoutes(this HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                            GlobalConstants.DefaultHttpRoute,
                            "api/{controller}/{action}/{id}",
                            new { id = RouteParameter.Optional });
        }

        private static void RegisterDependencies(this HttpConfiguration config)
        {
            config.DependencyResolver = new UnityResolver(Bootstrapper.Container);     
        }

        private static void EnableSwagger(this HttpConfiguration config)
        {
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "Skeleton Api")
                 .Description("Skeleton API for coding REST operations")
                 .TermsOfService("NA")
                 .Contact(cc => cc
                        .Name("Jej666")
                        .Email("jej666@gmail.com"));
                 //.License(lc => lc
                 //       .Name("Some License")
                 //       .Url("http://tempuri.org/license"));
                  
            })
                  .EnableSwaggerUi();
        }

        private static void RegisterFormatters(this HttpConfiguration config)
        {
            // Remove application/ x - www - form - urlencoded formatters
            var mediaTypeFormatters = config.Formatters
                .Where(y => y.SupportedMediaTypes
                .Any(c => c.MediaType == "application/x-www-form-urlencoded"))
                .ToList();
            mediaTypeFormatters.ForEach(x => config.Formatters.Remove(x));

            // Remove Xml Formatter
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Json formatter
            config.Formatters
                  .JsonFormatter
                  .SupportedMediaTypes
                  .Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}