using Microsoft.Owin;
using Skeleton.Web.Server.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Skeleton.Web.Server.Middlewares
{
    // Adapted from https://github.com/mikegore1000/SqueezeMe
    public class CompressionMiddleware
    {
        private const string AcceptEncoding = "Accept-Encoding";
        private readonly Func<IDictionary<string, object>, Task> next;
        private readonly OwinCompression compression = new OwinCompression();

        private readonly List<ICompressor> compressors = new List<ICompressor>()
        {
            new GZipCompressor(),
            new DeflateCompressor()
        };

        public CompressionMiddleware(Func<IDictionary<string, object>, Task> next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var context = new OwinContext(environment);
            var httpOutputStream = context.Response.Body;
            var compressor = GetCompressor(context.Request);

            if (compressor == null)
            {
                await next.Invoke(environment);
                return;
            }

            await compression.Compress(next, context, compressor, httpOutputStream);
            context.Response.Body = httpOutputStream;
        }

        private ICompressor GetCompressor(IOwinRequest request)
        {
            if (!request.Headers.ContainsKey(AcceptEncoding))
            {
                return null;
            }

            return (from c in compressors
                    from e in request.Headers.GetCommaSeparatedValues(AcceptEncoding)
                                     .Select(x => StringWithQualityHeaderValue.Parse(x))
                    orderby e.Quality descending
                    where string.Compare(c.ContentEncoding, e.Value, StringComparison.OrdinalIgnoreCase) == 0
                    select c)
                    .FirstOrDefault();
        }
    }
}
