using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Skeleton.Tests.Web
{
    public abstract class HttpTestBase
    {
        private static OwinServer Server = new OwinServer();

        protected HttpTestBase()
        {
            Server.Start();
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            Server.Dispose();
        }
    }
}
