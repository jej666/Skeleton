using Skeleton.Abstraction;
using System;

namespace Skeleton.Infrastructure.Logging
{
    public sealed class LoggerFactory : ILoggerFactory
    {
        public ILogger GetLogger(Type type)
        {
            return new Logger(type);
        }
    }
}