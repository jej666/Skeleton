using NUnit.Framework;
using Skeleton.Tests.Common;

namespace Skeleton.Tests.Web
{
    [SetUpFixture]
    public class HttpClientTestsSetup
    {
        private OwinTestServer _server;

        [OneTimeSetUp]
        public void Init()
        {
            _server = new OwinTestServer();
            _server.Start(AppConfiguration.BaseAddress);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _server?.Dispose();
        }
    }
}