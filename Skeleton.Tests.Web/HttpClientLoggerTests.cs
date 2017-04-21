using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skeleton.Web.Client;

namespace Skeleton.Tests.Web
{
    [TestClass]
    public class HttpClientLoggerTests
    {
        private readonly static JsonHttpClient Client =
           new JsonHttpClient(new RestUriBuilder(AppConfiguration.Host, "api/log", AppConfiguration.Port));

        [TestMethod]
        public void Logger_LogInfo()
        {
            var uri = Client.UriBuilder.StartNew().AppendAction("info").Uri;
            var response = Client.Post(uri, "This is an INFO log message");

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void Logger_LogWarn()
        {
            var uri = Client.UriBuilder.StartNew().AppendAction("warn").Uri;
            var response = Client.Post(uri, "This is a WARN log message");

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void Logger_LogDebug()
        {
            var uri = Client.UriBuilder.StartNew().AppendAction("debug").Uri;
            var response = Client.Post(uri, "This is a DEBUG log message");

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void Logger_LogError()
        {
            var uri = Client.UriBuilder.StartNew().AppendAction("error").Uri;
            var response = Client.Post(uri, "This is an ERROR log message");

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}