namespace Skeleton.Infrastructure.Data
{
    using Configuration;
    using System;

    public interface IDatabaseFactory
    {
        IDatabase CreateDatabase(Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator);

        IDatabaseAsync CreateDatabaseForAsyncOperations(Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator);
    }
}