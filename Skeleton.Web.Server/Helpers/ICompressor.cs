using System.IO;

namespace Skeleton.Web.Server.Helpers
{
    public interface ICompressor
    {
        string ContentEncoding { get; }

        Stream CreateStream(Stream destination);
    }
}