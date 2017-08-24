using NUnit.Framework;
using Skeleton.Tests.Common;
using Skeleton.Web.Client;

namespace Skeleton.Tests.Web
{
    [TestFixture]
    public class HttpClientLoggerTests
    {
        private readonly RestClient _client = new RestClient(AppConfiguration.BaseAddress, new NoRetryPolicy());

        [Test]
        public void Logger_LogInfo()
        {
            var response = _client.Post(
                 r => r.AddResource("api/log/info")
                       .WithBody("This is an INFO log message"));

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public void Logger_LogWarn()
        {
            var response = _client.Post(
                r => r.AddResource("api/log/warn")
                      .WithBody("This is a WARN log message"));

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public void Logger_LogDebug()
        {
            var response = _client.Post(
                 r => r.AddResource("api/log/debug")
                       .WithBody("This is a DEBUG log message"));

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public void Logger_LogError()
        {
            var response = _client.Post(
                 r => r.AddResource("api/log/error")
                       .WithBody("This is an ERROR log message"));

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}