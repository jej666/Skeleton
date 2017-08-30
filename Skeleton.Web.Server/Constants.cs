namespace Skeleton.Web.Server
{
    internal static class Constants
    {
        internal static class Messages
        {
            internal const string DefaultErrorMessage = "Oops! Sorry! Something went wrong.";
        }

        internal static class Headers
        {
            internal const string AcceptEncoding = "Accept-Encoding";
            internal const string ContentEncoding = "Content-Encoding";
            internal const string RequestID = "X-Request-ID";
        }

        internal static class OwinEnvironment
        {
            internal const string Context = "MS_OwinContext";
            internal const string RequestId = "owin.RequestId";
        }

        internal static class Routes
        {
            internal const string DefaultRpcRoute = "DefaultRpcRoute";
            internal const string DefaultApiPath = "api";
            internal const string DefaultRpcRouteTemplate = DefaultApiPath + "/{controller}/{action}/{id}";
        }
    }
}