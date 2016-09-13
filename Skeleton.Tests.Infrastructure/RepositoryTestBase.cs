using Skeleton.Abstraction;
using Skeleton.Infrastructure.DependencyInjection;

namespace Skeleton.Tests.Infrastructure
{
    public abstract class RepositoryTestBase
    {
        protected RepositoryTestBase()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();
            Bootstrapper.Initialize();
            Bootstrapper.UseDatabase(builder =>
                    builder.UsingConfigConnectionString("Default").Build());
        }

        protected static IDependencyResolver Container => Bootstrapper.Resolver;
    }
}