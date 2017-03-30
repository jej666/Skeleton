using Skeleton.Common;
using Swashbuckle.Application;
using System.Linq;
using System.Web.Http;

namespace Skeleton.Web.Server.Configuration
{
    public static class SwaggerConfig
    {
        public static void RegisterSwagger(this HttpConfiguration configuration)
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
                //.License(lc => lc
                //       .Name("Some License")
                //       .Url("http://tempuri.org/license"));

                c.SchemaId(t => t.FullName.Contains('`') ? t.FullName.Substring(0, t.FullName.IndexOf('`')) : t.FullName);
                c.IgnoreObsoleteProperties();
                c.DescribeAllEnumsAsStrings();
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.CustomProvider((defaultProvider) => new CachingSwaggerProvider(defaultProvider));
            })
                  .EnableSwaggerUi();
        }
    }
}