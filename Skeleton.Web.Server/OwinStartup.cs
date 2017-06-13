using Microsoft.Owin.Hosting;
using Skeleton.Abstraction.Dependency;
using Skeleton.Common;
using System;

namespace Skeleton.Web.Server
{
    public static class OwinStartup
    {
        public static IDisposable StartServer(Uri url, Action<IOwinBootstrapper> bootstrap)
        {
            url.ThrowIfNull(nameof(url));
            bootstrap.ThrowIfNull(nameof(bootstrap));

            var bootstrapper = new OwinBootstrapper();
            
            bootstrap(bootstrapper);
            
            return WebApp.Start<OwinBootstrapper>(url.ToString());
        }
    }
}