using Skeleton.Common;
using Skeleton.Infrastructure.DependencyResolver;

namespace Skeleton.Tests
{
    public abstract class TestBase
    {
        public TestBase()
        {
            Bootstrapper.Initialize();
        }

        public IContainer Container
        {
            get { return Bootstrapper.Container; }
        }
    }
}