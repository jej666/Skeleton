namespace Skeleton.Web.Server
{
    public sealed class OwinServerOptions
    {
        public bool RequireSsl { get; set; }
        public bool EnableCompression { get; set; }
        public bool EnableRequestId { get; set; }
        public bool EnableRequestLogging { get; set; }
        public bool ValidateModel { get; set; }
        public bool EnableSwagger { get; set; }
        public bool EnableGlobalExceptionHandling { get; set; }

        public OwinServerOptions()
        {
            RequireSsl = false;
            EnableCompression = true;
            EnableRequestId = true;
            EnableRequestLogging = true;
            EnableSwagger = true;
            EnableGlobalExceptionHandling = true;
            ValidateModel = true;
        }
    }
}
