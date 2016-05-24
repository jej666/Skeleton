using System;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Logging
{
    public static class LoggerFactory
    {
        public static ILogger GetLogger(Type type)
        {
            return new Logger(type);
        }
    }
}