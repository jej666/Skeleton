using System;

namespace Skeleton.Abstraction.Data
{
    public interface IDatabaseFactory
    {
        IDatabase CreateDatabase(
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator);

        IDatabaseAsync CreateDatabaseForAsyncOperations(
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator);
    }
}