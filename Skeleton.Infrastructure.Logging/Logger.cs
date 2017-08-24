using log4net;
using Skeleton.Abstraction;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Skeleton.Infrastructure.Logging
{
    public sealed class Logger : ILogger
    {
        private readonly ILog _log;

        public Logger(Type type)
        {
            _log = LogManager.GetLogger(type);
        }

        public void Debug(object message)
        {
            _log.Debug(message);
        }

        public void Debug(object message, Exception ex)
        {
            _log.Debug(message, ex);
        }

        public void DebugFormat(string format, params object[] args)
        {
            _log.DebugFormat(CultureInfo.CurrentCulture, format, args);
        }

        public void Error(object message)
        {
            _log.Error(message);
        }

        public void Error(object message, Exception ex)
        {
            _log.Error(message, ex);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            _log.ErrorFormat(CultureInfo.CurrentCulture, format, args);
        }

        public void Fatal(object message)
        {
            _log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            _log.FatalFormat(CultureInfo.CurrentCulture, format, args);
        }

        public void Info(object message)
        {
            _log.Info(message);
        }

        public void Info(object message, Exception ex)
        {
            _log.Info(message, ex);
        }

        public void InfoFormat(string format, params object[] args)
        {
            _log.InfoFormat(CultureInfo.CurrentCulture, format, args);
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }

        public void Warn(object message, Exception ex)
        {
            _log.Warn(message, ex);
        }

        public void WarnFormat(string format, params object[] args)
        {
            _log.WarnFormat(CultureInfo.CurrentCulture, format, args);
        }
    }
}