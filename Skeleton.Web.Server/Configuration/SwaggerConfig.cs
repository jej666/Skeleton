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
                c.IgnoreObsoleteProperties();
                c.DescribeAllEnumsAsStrings();
            })
                  .EnableSwaggerUi();
        }
    }
}