using Skeleton.Core;
using Skeleton.Infrastructure.DependencyResolver;
using Skeleton.Tests.Infrastructure;

namespace Skeleton.Tests
{
    public abstract class TestBase
    {
        protected TestBase()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();
            Bootstrapper.Initialize();
            Bootstrapper.UseDatabase(builder =>
                    builder.UsingConfigConnectionString("Default").Build());
        }

        protected static IDependencyResolver Container
        {
            get { return Bootstrapper.Resolver; }
        }
    }
}