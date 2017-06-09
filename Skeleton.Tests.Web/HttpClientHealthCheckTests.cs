using NUnit.Framework;
using Skeleton.Tests.Common;
using Skeleton.Web.Client;

namespace Skeleton.Tests.Web
{
    [TestFixture]
    public class HttpClientHealthCheckTests
    {
        private readonly static JsonHttpClient Client =
           new JsonHttpClient(
               new RestUriBuilder(AppConfiguration.Host, "api/HealthCheck", AppConfiguration.Port));

        [Test]
        public void HeartBeat()
        {
            var uri = Client.UriBuilder.StartNew().Uri;
            var response = Client.Get(uri);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}