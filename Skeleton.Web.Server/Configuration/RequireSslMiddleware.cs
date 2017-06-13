using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Web.Server.Owin
{
    public class RequireSslMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> _next;

        public RequireSslMiddleware(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);

            if (context.Request.Uri.Scheme != Uri.UriSchemeHttps)
            {
                context.Response.StatusCode = 403;
                context.Response.ReasonPhrase = "SSL is required.";

                return;
            }

            await _next(env);
        }
    }
}