using Skeleton.Common;
using Skeleton.Infrastructure.DependencyInjection;
using System.Net.Http.Headers;
using System.Linq;
using System.Web.Http;

namespace Skeleton.Web.Server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.ThrowIfNull(() => config);

            config.DependencyResolver = new UnityResolver(Bootstrapper.Container);
            config.ConfigureFormatters();

            config.Filters.Add(new CheckModelForNullAttribute());
            config.Filters.Add(new ValidateModelStateAttribute());

            config.Routes.MapHttpRoute(
                "DefaultApiWithId",
                "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional });
        }

        private static void ConfigureFormatters(this HttpConfiguration config)
        {
            // Remove application/ x - www - form - urlencoded formatters
                  var mediaTypeFormatters = config.Formatters
                      .Where(y => y.SupportedMediaTypes.Any(c => c.MediaType == "application/x-www-form-urlencoded"))
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