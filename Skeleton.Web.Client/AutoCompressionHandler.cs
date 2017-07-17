using System.Net;
using System.Net.Http;

namespace Skeleton.Web.Client
{
    public sealed class AutomaticDecompressionHandler : HttpClientHandler
    {
        public AutomaticDecompressionHandler()
        {
            if (SupportsAutomaticDecompression)
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
        }
    }
}