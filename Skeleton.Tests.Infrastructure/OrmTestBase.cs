using Skeleton.Abstraction;
using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Tests.Common;

namespace Skeleton.Tests.Infrastructure
{
    public abstract class OrmTestBase
    {
        protected OrmTestBase()
        {
            SqlLocalDbHelper.CreateDatabaseIfNotExists();

            Bootstrapper.UseDatabase(
                builder => builder.UsingConnectionString(
                    AppConfiguration.ConnectionString)
                    .Build());
        }

        protected static IDependencyResolver Container => Bootstrapper.Resolver;
    }
}