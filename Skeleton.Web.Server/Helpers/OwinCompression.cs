using Microsoft.Owin;
using System.IO;
using System.Threading.Tasks;

namespace Skeleton.Web.Server.Helpers
{
    internal class OwinCompression
    {
        private const int BufferSize = 8192;

        internal async Task Compress(
            OwinMiddleware next,
            IOwinContext context,
            ICompressor compressor,
            Stream httpOutputStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var compressedStream = compressor.CreateStream(memoryStream))
                {
                    context.Response.Body = compressedStream;
                    await next.Invoke(context);
                }

                if (memoryStream.Length > 0)
                {
                    SetResponseHeaders(context, compressor, memoryStream);
                    memoryStream.Position = 0;

                    await memoryStream.CopyToAsync(httpOutputStream, BufferSize);
                }
            }
        }

        private static void SetResponseHeaders(
            IOwinContext context,
            ICompressor compressor,
            MemoryStream memoryStream)
        {
            context.Response.Headers[Constants.Headers.ContentEncoding] = compressor.ContentEncoding;
            context.Response.ContentLength = memoryStream.Length;
        }
    }
}