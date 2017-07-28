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
               new RestUriBuilder(AppConfiguration.Host, AppConfiguration.Port, "api/HealthCheck"));

        [Test]
        public void HeartBeat()
        {
            var uri = Client.UriBuilder.Uri;
            var response = Client.Get(uri);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public void Discover_HeartBeat()
        {
            ClientProvider.RegisterServices(AppConfiguration.BaseUrl);

            var client = ClientProvider.FindClient("healthcheck");
            var response = client.Get(client.UriBuilder.Uri);

            Assert.IsNotNull(response);
        }

    }
}