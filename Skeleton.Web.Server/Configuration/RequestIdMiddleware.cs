using Microsoft.Owin;
using Skeleton.Core;
using Skeleton.Web.Server.Helpers;
using System.Threading.Tasks;

namespace Skeleton.Web.Server.Configuration
{
    public sealed class RequestIdMiddleware: OwinMiddleware
    {
        public RequestIdMiddleware(OwinMiddleware next ): base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            var requestId = context.GetRequestId();

            if (requestId.IsNotNullOrEmpty())
                context.SetRequestId(requestId);

            await Next.Invoke(context);
        }
    }
}
