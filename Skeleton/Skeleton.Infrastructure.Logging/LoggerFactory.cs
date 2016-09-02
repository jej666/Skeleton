using System;
using Skeleton.Core;

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