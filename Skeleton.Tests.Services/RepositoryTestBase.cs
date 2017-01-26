using Skeleton.Abstraction;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Tests.Common;

namespace Skeleton.Tests.Services
{
    public abstract class RepositoryTestBase
    {
        protected RepositoryTestBase()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();
          //  SqlLocalDbHelper.InstallProcStocIfNotExists();
            Bootstrapper.Initialize();
            Bootstrapper.UseDatabase(builder =>
                    builder.UsingConfigConnectionString("Default").Build());
        }

        protected static IDependencyResolver Container => Bootstrapper.Resolver;
    }
}