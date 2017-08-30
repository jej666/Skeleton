using Microsoft.Owin;
using System;
using System.Threading.Tasks;

namespace Skeleton.Web.Server.Configuration
{
    public sealed class RequireSslMiddleware: OwinMiddleware
    {
        private readonly OwinMiddleware _next;

        public RequireSslMiddleware(OwinMiddleware next)
            : base(next)
        {   
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Uri.Scheme != Uri.UriSchemeHttps)
            {
                context.Response.StatusCode = 403;
                context.Response.ReasonPhrase = "SSL is required.";

                await context.Response.WriteAsync(context.Response.ReasonPhrase);

                return;
            }

            await Next.Invoke(context);
        }
    }
}