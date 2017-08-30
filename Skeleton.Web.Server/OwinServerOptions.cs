namespace Skeleton.Web.Server
{
    public sealed class OwinServerOptions
    {
        public bool RequireSsl { get; set; }
        public bool EnableCompression { get; set; }
        public bool EnableRequestId { get; set; }
        public bool EnableRequestLogging { get; set; }
    }
}
