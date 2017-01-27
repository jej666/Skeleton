using Skeleton.Abstraction;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Tests.Common;

namespace Skeleton.Tests.Repository
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