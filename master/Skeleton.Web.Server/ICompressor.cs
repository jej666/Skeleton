using System.IO;

namespace Skeleton.Web.Server
{
    public interface ICompressor
    {
        string ContentEncoding { get; }

        Stream CreateStream(Stream destination);
    }
}