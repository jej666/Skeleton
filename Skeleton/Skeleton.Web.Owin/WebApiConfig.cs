using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Skeleton.Web.Owin
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "WebApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}