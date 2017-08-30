using Microsoft.Owin;
using Skeleton.Web.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Skeleton.Web.Server.Configuration
{
    // Adapted from https://github.com/mikegore1000/SqueezeMe
    public sealed class CompressionMiddleware: OwinMiddleware
    {
        private readonly OwinCompression compression = new OwinCompression();
        private readonly List<ICompressor> compressors = new List<ICompressor>
        {
            new GZipCompressor(),
            new DeflateCompressor()
        };

        public CompressionMiddleware(OwinMiddleware next)
            :base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            var httpOutputStream = context.Response.Body;
            var compressor = GetCompressor(context.Request);

            if (compressor == null)
            {
                await Next.Invoke(context);
                return;
            }

            await compression.Compress(Next, context, compressor, httpOutputStream);

            context.Response.Body = httpOutputStream;
        }

        private ICompressor GetCompressor(IOwinRequest request)
        {
            if (!request.Headers.ContainsKey(Constants.Headers.AcceptEncoding))
                return null;

            return (from c in compressors
                    from e in request.Headers.GetCommaSeparatedValues(Constants.Headers.AcceptEncoding)
                                     .Select(x => StringWithQualityHeaderValue.Parse(x))
                    orderby e.Quality descending
                    where string.Compare(c.ContentEncoding, e.Value, StringComparison.OrdinalIgnoreCase) == 0
                    select c)
                    .FirstOrDefault();
        }
    }
}