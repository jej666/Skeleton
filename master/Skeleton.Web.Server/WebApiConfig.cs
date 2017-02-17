﻿using Skeleton.Common;
using Skeleton.Infrastructure.DependencyInjection;
using Swashbuckle.Application;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Skeleton.Web.Server
{
    public static class WebApiConfig
    {
        public static void Register(this HttpConfiguration configuration)
        {
            configuration.ThrowIfNull(() => configuration);

            configuration.RegisterDependencies();
            configuration.RegisterFormatters();
            configuration.RegisterFilters();
            configuration.RegisterRoutes();
            configuration.EnsureInitialized();
            configuration.EnableSwagger();
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

        private static void EnableSwagger(this HttpConfiguration configuration)
        {
            configuration.EnableSwagger(c =>
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

        private static void RegisterFormatters(this HttpConfiguration configuration)
        {
            configuration.Formatters.Clear();
            configuration.Formatters.Add(new JsonMediaTypeFormatter());
        }
    }
}