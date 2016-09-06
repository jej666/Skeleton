using System;
using Skeleton.Infrastructure.Data.Configuration;

namespace Skeleton.Infrastructure.Data
{
    public interface IDatabaseFactory
    {
        IDatabase CreateDatabase(Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator);

        IDatabaseAsync CreateDatabaseForAsyncOperations(
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator);
    }
}