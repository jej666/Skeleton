using System.Net.Http.Headers;
using System.Web.Http;
using Skeleton.Infrastructure.DependencyResolver;

namespace Skeleton.Web.Server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //string alphanumeric = @"^[a-zA-Z]+[a-zA-Z0-9_]*$";
            //string numeric = @"^\d+$";

            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));

            config.DependencyResolver = new UnityResolver(Bootstrapper.Container);

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
                );

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApiControllerActionName",
            //    routeTemplate: "api/{controller}/{id}/{id2}",
            //    defaults: null
            //    );

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApiControllerPage",
            //    routeTemplate: "api/{controller}/{action}/{pageSize}/{pageNumber}",
            //    defaults: null,
            //    constraints: new { 
            //        httpMethod = new HttpMethodConstraint(HttpMethod.Get), 
            //        pageSize = numeric,
            //        pageNumber = numeric
            //    }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApiControllerActionId",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: null,
            //    constraints: new { action = alphanumeric, id = numeric } // action must start with character
            //);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApiControllerActionName",
            //    routeTemplate: "api/{controller}/{action}/{name}",
            //    defaults: null,
            //    constraints: new { action = alphanumeric, name = alphanumeric } // action and name must start with character
            //);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApiControllerAction",
            //    routeTemplate: "api/{controller}/{action}",
            //    defaults: null,
            //    constraints: new { action = alphanumeric } // action must start with character
            //);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApiControllerGet",
            //    routeTemplate: "api/{controller}",
            //    defaults: new { action = "Get" },
            //    constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApiControllerPost",
            //    routeTemplate: "api/{controller}",
            //    defaults: new { action = "Post" },
            //    constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApiControllerPut",
            //    routeTemplate: "api/{controller}",
            //    defaults: new { action = "Put" },
            //    constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Put) }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApiControllerDelete",
            //    routeTemplate: "api/{controller}",
            //    defaults: new { action = "Delete" },
            //    constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Delete) }
            //);
        }
    }
}