using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Skeleton.Web.Server.Owin
{
    internal class OwinCompression
    {
        private const int BufferSize = 8192;

        internal async Task Compress(
            Func<IDictionary<string, object>, Task> next,
            OwinContext context,
            ICompressor compressor,
            Stream httpOutputStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var compressedStream = compressor.CreateStream(memoryStream))
                {
                    context.Response.Body = compressedStream;
                    await next.Invoke(context.Environment);
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
            OwinContext context,
            ICompressor compressor,
            MemoryStream memoryStream)
        {
            context.Response.Headers[Constants.ContentEncoding] = compressor.ContentEncoding;
            context.Response.ContentLength = memoryStream.Length;
        }
    }
}