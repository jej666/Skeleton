using System;
using Skeleton.Abstraction;

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