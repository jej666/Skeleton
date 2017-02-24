using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Skeleton.Common;
using Skeleton.Infrastructure.DependencyInjection;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Skeleton.Web.Server.Configuration
{
    public static class WebApiConfig
    {
        public static void RegisterWebApi(this HttpConfiguration configuration)
        {
            configuration.ThrowIfNull(() => configuration);

            configuration
                .RegisterDependencies()
                .RegisterFormatters()
                .RegisterFilters()
                .RegisterRoutes()
                .EnsureInitialized();
        }

        private static HttpConfiguration RegisterFilters(this HttpConfiguration configuration)
        {
            configuration.Filters.Add(new CheckModelForNullAttribute());
            configuration.Filters.Add(new ValidateModelStateAttribute());

            return configuration;
        }

        private static HttpConfiguration RegisterRoutes(this HttpConfiguration configuration)
        {
            configuration.MapHttpAttributeRoutes();

            configuration.Routes.MapHttpRoute(
                            name: Constants.DefaultHttpRoute,
                            routeTemplate: "api/{controller}/{action}/{id}",
                            defaults: new { id = RouteParameter.Optional });

            return configuration;
        }

        private static HttpConfiguration RegisterDependencies(this HttpConfiguration configuration)
        {
            configuration.DependencyResolver = new UnityResolver(Bootstrapper.Container);

            return configuration;
        }

        private static HttpConfiguration RegisterFormatters(this HttpConfiguration configuration)
        {
            var defaultSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            JsonConvert.DefaultSettings = () => { return defaultSettings; };

            configuration.Formatters.Clear();
            configuration.Formatters.Add(new JsonMediaTypeFormatter());
            configuration.Formatters.JsonFormatter.SerializerSettings = defaultSettings;

            return configuration;
        }
    }
}