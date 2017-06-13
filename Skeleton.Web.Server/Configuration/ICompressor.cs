using System.IO;

namespace Skeleton.Web.Server.Configuration
{
    public interface ICompressor
    {
        string ContentEncoding { get; }

        Stream CreateStream(Stream destination);
    }
}