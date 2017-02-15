﻿using Skeleton.Common;
using Skeleton.Infrastructure.DependencyInjection;
using Swashbuckle.Application;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Skeleton.Web.Server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.ThrowIfNull(() => config);

            config.Formatters
                  .JsonFormatter
                  .SupportedMediaTypes
                  .Add(new MediaTypeHeaderValue("text/html"));

            config.DependencyResolver = new UnityResolver(Bootstrapper.Container);

            config.Filters.Add(new CheckModelForNullAttribute());
            config.Filters.Add(new ValidateModelStateAttribute());

            config.Routes.MapHttpRoute(
                "DefaultApiWithId",
                "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional });

            config.EnableSwagger(c => c.SingleApiVersion("v1", "Skeleton API"))
                  .EnableSwaggerUi();
        }
    }
}