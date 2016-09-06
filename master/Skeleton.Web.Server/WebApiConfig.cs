using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Routing;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Web.Server.Filters;

namespace Skeleton.Web.Server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));

            config.DependencyResolver = new UnityResolver(Bootstrapper.Container);

            config.Filters.Add(new CheckModelForNullAttribute());
            config.Filters.Add(new ValidateModelStateAttribute());

            config.Routes.MapHttpRoute("DefaultApiWithId", "api/{controller}/{id}", new {id = RouteParameter.Optional},
                new {id = @"\d+"});
            config.Routes.MapHttpRoute("DefaultApiWithAction", "api/{controller}/{action}");
            config.Routes.MapHttpRoute("DefaultApiGet", "api/{controller}", new {action = "Get"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Get)});
            config.Routes.MapHttpRoute("DefaultApiPost", "api/{controller}", new {action = "Post"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Post)});
            config.Routes.MapHttpRoute("DefaultApiPut", "api/{controller}", new {action = "Put"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Put)});
            config.Routes.MapHttpRoute("DefaultApiDelete", "api/{controller}", new {action = "Delete"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Delete)});
        }
    }
}