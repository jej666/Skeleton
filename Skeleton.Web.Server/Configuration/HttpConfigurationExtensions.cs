using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Skeleton.Abstraction;
using Skeleton.Common;
using Swashbuckle.Application;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Skeleton.Web.Server.Configuration
{
    public static class HttpConfigurationExtensions
    {
        public static HttpConfiguration UseJson(this HttpConfiguration configuration)
        {
            configuration.ThrowIfNull(nameof(configuration));

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

        public static HttpConfiguration UseSwagger(this HttpConfiguration configuration)
        {
            configuration.ThrowIfNull(nameof(configuration));

            configuration
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

            return configuration;
        }

        public static HttpConfiguration UseCheckModelForNull(this HttpConfiguration configuration)
        {
            configuration.Filters.Add(new CheckModelForNullAttribute());

            return configuration;
        }

        public static HttpConfiguration UseValidateModelState(this HttpConfiguration configuration)
        {
            configuration.Filters.Add(new ValidateModelStateAttribute());

            return configuration;
        }

        public static HttpConfiguration UseGlobalExceptionHandling(this HttpConfiguration configuration)
        {
            var loggerFactory = configuration.DependencyResolver.GetService(typeof(ILoggerFactory)) as ILoggerFactory;

            configuration.Services.Add(typeof(IExceptionLogger), new GlobalExceptionLogger(loggerFactory));
            configuration.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

            return configuration;
        }
    }
}