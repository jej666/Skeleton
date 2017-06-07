using NUnit.Framework;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Startup;
using Skeleton.Infrastructure.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Skeleton.Tests.Infrastructure
{
    [TestFixture]
    public class LoggingTests
    {
        private readonly IBootstrapper _bootstrapper = new Bootstrapper();
        private readonly ILogger _logger;

        public LoggingTests()
        {
            _logger = _bootstrapper.Resolve<ILoggerFactory>().GetLogger(typeof(LoggingTests));
        }

        [Test]
        public void Log_Debug()
        {
            _logger.Debug("This is a debug test");
        }

        [Test]
        public void Log_DebugFormat()
        {
            _logger.DebugFormat("This is a debug test {0}", "with format");
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        [Test]
        public void Log_DebugException()
        {
            _logger.Debug("This is a debug test", new Exception("DummyException"));
        }

        [Test]
        public void Log_Error()
        {
            _logger.Error("This is a error test");
        }

        [Test]
        public void Log_ErrorFormat()
        {
            _logger.ErrorFormat("This is a error test {0}", "with format");
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        [Test]
        public void Log_ErrorException()
        {
            _logger.Error("This is a error test", new Exception("DummyException"));
        }

        [Test]
        public void Log_Fatal()
        {
            _logger.Fatal("This is a fatal test");
        }

        [Test]
        public void Log_FatalFormat()
        {
            _logger.FatalFormat("This is a fatal test {0}", "with format");
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        [Test]
        public void Log_FatalException()
        {
            _logger.Fatal("This is a fatal test", new Exception("DummyException"));
        }

        [Test]
        public void Log_Info()
        {
            _logger.Info("This is a info test");
        }

        [Test]
        public void Log_InfoFormat()
        {
            _logger.InfoFormat("This is a info test {0}", "with format");
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        [Test]
        public void Log_InfoException()
        {
            _logger.Info("This is a info test", new Exception("DummyException"));
        }

        [Test]
        public void Log_Warn()
        {
            _logger.Warn("This is a warn test");
        }

        [Test]
        public void Log_WarnFormat()
        {
            _logger.WarnFormat("This is a warn test {0}", "with format");
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        [Test]
        public void Log_WarnException()
        {
            _logger.Warn("This is a warn test", new Exception("DummyException"));
        }
    }
}