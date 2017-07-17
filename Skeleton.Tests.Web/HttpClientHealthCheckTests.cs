using NUnit.Framework;
using Skeleton.Tests.Common;
using Skeleton.Web.Client;

namespace Skeleton.Tests.Web
{
    [TestFixture]
    public class HttpClientHealthCheckTests
    {
        private readonly JsonHttpClient Client =
           new JsonHttpClient(
               new RestUriBuilder(AppConfiguration.Host, AppConfiguration.Port, "api/HealthCheck"),
               new AutomaticDecompressionHandler());

        [Test]
        public void HeartBeat()
        {
            var uri = Client.UriBuilder.StartNew().Uri;
            var response = Client.Get(uri);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}