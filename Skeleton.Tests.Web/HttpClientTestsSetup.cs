using NUnit.Framework;
using Skeleton.Tests.Web.Mock;

namespace Skeleton.Tests.Web
{
    [SetUpFixture]
    public class HttpClientTestsSetup
    {
        private OwinServer _server;

        [OneTimeSetUp]
        public void Init()
        {
            _server = new OwinServer();
            _server.Start(AppConfiguration.BaseUrl);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _server.Dispose();
        }
    }
}