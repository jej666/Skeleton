namespace Skeleton.Infrastructure.Logging
{
    using Common;
    using System;

    public static class LoggerFactory
    {
        public static ILogger GetLogger(Type type)
        {
            return new Logger(type);
        }
    }
}