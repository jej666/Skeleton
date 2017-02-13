using System;
using System.Diagnostics.CodeAnalysis;

namespace Skeleton.Abstraction
{
    public interface ILogger : IHideObjectMethods
    {
        void Debug(object message);

        void Debug(object message, Exception ex);

        void DebugFormat(string format, params object[] args);

        void Error(object message);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error")]
        void Error(object message, Exception ex);

        void ErrorFormat(string format, params object[] args);

        void Fatal(object message);

        void Fatal(object message, Exception exception);

        void FatalFormat(string format, params object[] args);

        void Info(object message);

        void Info(object message, Exception ex);

        void InfoFormat(string format, params object[] args);

        void Warn(object message);

        void Warn(object message, Exception ex);

        void WarnFormat(string format, params object[] args);
    }
}