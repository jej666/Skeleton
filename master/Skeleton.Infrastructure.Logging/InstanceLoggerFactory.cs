using Skeleton.Abstraction;
using System;

namespace Skeleton.Infrastructure.Logging
{
    public sealed class InstanceLoggerFactory: ILoggerFactory
    { 
        public ILogger GetLogger(Type type)
        {
            return new Logger(type);
        }
    }
}