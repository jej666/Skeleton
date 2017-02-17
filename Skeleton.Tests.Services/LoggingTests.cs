using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Abstraction;
using Skeleton.Infrastructure.DependencyInjection;
using System;

namespace Skeleton.Tests
{
    [TestClass]
    public class LoggingTests
    {
        private readonly ILogger _logger = Bootstrapper.Resolver.Resolve<ILoggerFactory>().GetLogger(typeof(LoggingTests));

        [TestMethod]
        public void Log_Debug()
        {
            _logger.Debug("This is a debug test");
        }

        [TestMethod]
        public void Log_DebugFormat()
        {
            _logger.DebugFormat("This is a debug test {0}", "with format");
        }

        [TestMethod]
        public void Log_DebugException()
        {
            _logger.Debug("This is a debug test", new Exception("DummyException"));
        }

        [TestMethod]
        public void Log_Error()
        {
            _logger.Error("This is a error test");
        }

        [TestMethod]
        public void Log_ErrorFormat()
        {
            _logger.ErrorFormat("This is a error test {0}", "with format");
        }

        [TestMethod]
        public void Log_ErrorException()
        {
            _logger.Error("This is a error test", new Exception("DummyException"));
        }

        [TestMethod]
        public void Log_Fatal()
        {
            _logger.Fatal("This is a fatal test");
        }

        [TestMethod]
        public void Log_FatalFormat()
        {
            _logger.FatalFormat("This is a fatal test {0}", "with format");
        }

        [TestMethod]
        public void Log_FatalException()
        {
            _logger.Fatal("This is a fatal test", new Exception("DummyException"));
        }

        [TestMethod]
        public void Log_Info()
        {
            _logger.Info("This is a info test");
        }

        [TestMethod]
        public void Log_InfoFormat()
        {
            _logger.InfoFormat("This is a info test {0}", "with format");
        }

        [TestMethod]
        public void Log_InfoException()
        {
            _logger.Info("This is a info test", new Exception("DummyException"));
        }

        [TestMethod]
        public void Log_Warn()
        {
            _logger.Warn("This is a warn test");
        }

        [TestMethod]
        public void Log_WarnFormat()
        {
            _logger.WarnFormat("This is a warn test {0}", "with format");
        }

        [TestMethod]
        public void Log_WarnException()
        {
            _logger.Warn("This is a warn test", new Exception("DummyException"));
        }
    }
}
