using Skeleton.Web.Server.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Skeleton.Web.Server.Controllers
{
    public class DiscoverController : ApiController
    {
        [HttpGet]
        public IEnumerable<ServiceRegistry> Get()
        {
            var apiDescriptions = Configuration.Services.GetApiExplorer().ApiDescriptions;
            var apiServices = apiDescriptions
                .GroupBy(s => s.ActionDescriptor.ControllerDescriptor.ControllerName)
                .Select(c => c.FirstOrDefault())
                .ToList();

            return apiServices.Select(api =>
            {
               var name = api.ActionDescriptor.ControllerDescriptor.ControllerName;
               var builder = Request.CreateUriBuilder();
               builder.Path = Constants.Routes.DefaultApiPath + "/" + name;

               return new ServiceRegistry
               {
                   Name = name,
                   Host = builder.Uri.ToString()
               };
            });
        }
    }

    public class ServiceRegistry
    {
        public string Name { get; set; }
        public string Host { get; set; }
    }
}