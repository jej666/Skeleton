using NUnit.Framework;
using Skeleton.Tests.Common;
using Skeleton.Web.Client;
using System;

namespace Skeleton.Tests.Web
{
    [TestFixture]
    public class HttpClientHealthCheckTests
    {
        private readonly RestClient Client =  new RestClient(AppConfiguration.BaseAddress);

        [Test]
        public void HeartBeat()
        {
            var response = Client.Get(r => r.AddResource("api/healthcheck/heartbeat"));

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        //[Test]
        //public void Discover_HeartBeat()
        //{
        //    ClientProvider.RegisterServices(AppConfiguration.BaseAddress);

        //    var client = ClientProvider.GetClient("healthcheck");
        //    var response = client.Get(client.UriBuilder.Uri);

        //    Assert.IsNotNull(response);
        //}
    }
}