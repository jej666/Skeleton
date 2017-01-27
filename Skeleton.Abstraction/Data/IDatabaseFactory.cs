using System;

namespace Skeleton.Abstraction.Data
{
    public interface IDatabaseFactory
    {
        IDatabase CreateDatabase(
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator);

        IAsyncDatabase CreateDatabaseForAsyncOperations(
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator);
    }
}