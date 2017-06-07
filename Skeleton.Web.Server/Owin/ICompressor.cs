using System.IO;

namespace Skeleton.Web.Server.Owin
{
    public interface ICompressor
    {
        string ContentEncoding { get; }

        Stream CreateStream(Stream destination);
    }
}