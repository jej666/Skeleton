using System;

namespace Skeleton.Abstraction
{
    public interface ILoggerFactory
    {
        ILogger GetLogger(Type type);
    }
}
