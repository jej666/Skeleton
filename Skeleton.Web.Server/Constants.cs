namespace Skeleton.Web.Server
{
    internal static class Constants
    {
        internal const string DefaultHttpRoute = "DefaultApiRoute";
        internal const string DefaultApiPath = "api";
        internal const string DefaultRouteTemplate = DefaultApiPath + "/{controller}/{action}/{id}";
        internal const string DefaultErrorMessage = "Oops! Sorry! Something went wrong.";
        internal const string AcceptEncoding = "Accept-Encoding";
        internal const string ContentEncoding = "Content-Encoding";
        internal const string OwinContext = "MS_OwinContext";
    }
}