using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Skeleton.Common;
using Skeleton.Infrastructure.DependencyInjection;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Skeleton.Web.Server
{
    public static class WebApiConfig
    {
        public static void RegisterWebApi(this HttpConfiguration configuration)
        {
            configuration.ThrowIfNull(() => configuration);

            configuration.RegisterDependencies();
            configuration.RegisterFormatters();
            configuration.RegisterFilters();
            configuration.RegisterRoutes();
            configuration.EnsureInitialized();
        }

        private static void RegisterFilters(this HttpConfiguration configuration)
        {
            configuration.Filters.Add(new CheckModelForNullAttribute());
            configuration.Filters.Add(new ValidateModelStateAttribute());
        }

        private static void RegisterRoutes(this HttpConfiguration configuration)
        {
            configuration.MapHttpAttributeRoutes();

            configuration.Routes.MapHttpRoute(
                            Constants.DefaultHttpRoute,
                            "api/{controller}/{action}/{id}",
                            new { id = RouteParameter.Optional });
        }

        private static void RegisterDependencies(this HttpConfiguration configuration)
        {
            configuration.DependencyResolver = new UnityResolver(Bootstrapper.Container);
        }

        private static void RegisterFormatters(this HttpConfiguration configuration)
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
        }
    }
}