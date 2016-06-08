using System;
using System.Threading.Tasks;
using Owin;
using System.Web.Http;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Skeleton.Web.Owin.Startup))]

namespace Skeleton.Web.Owin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            app.UseWebApi(httpConfiguration);
        }
    }
}
