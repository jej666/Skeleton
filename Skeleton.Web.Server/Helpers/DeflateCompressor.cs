﻿using System.IO;
using System.IO.Compression;

namespace Skeleton.Web.Server.Helpers
{
    public class DeflateCompressor : ICompressor
    {
        public string ContentEncoding => "deflate";

        public Stream CreateStream(Stream destination)
        {
            return new DeflateStream(destination, CompressionLevel.Fastest, leaveOpen: true);
        }
    }
}