using System.IO;

namespace Skeleton.Web.Server.Compression
{
    public interface ICompressor
    {
        string ContentEncoding { get; }

        Stream CreateStream(Stream destination);
    }
}
