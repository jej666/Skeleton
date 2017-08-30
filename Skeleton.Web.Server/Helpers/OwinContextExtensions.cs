using Microsoft.Owin;

namespace Skeleton.Web.Server.Helpers
{
    public static class OwinContextExtensions
    {
        public static string GetRequestId(this IOwinContext context)
        {
            object id;
            context.Environment.TryGetValue(Constants.OwinEnvironment.RequestId, out id);

            return id.ToString();
        }

        public static void SetRequestId(this IOwinContext context, string requestId)
        {
            context.Response.Headers.Add(Constants.Headers.RequestID, new string[] { requestId });
        }
    }
}