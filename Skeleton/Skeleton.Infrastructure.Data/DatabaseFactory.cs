using System;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;

namespace Skeleton.Infrastructure.Data
{
    public sealed class DatabaseFactory : IDatabaseFactory
    {
        private readonly IDatabaseConfigurationBuilder _configurationBuilder;
        private readonly ILogger _logger;
        private readonly IMetadataProvider _typeAccessorCache;

        public DatabaseFactory(IMetadataProvider typeAccessorCache, ILogger logger)
        {
            _configurationBuilder = new DatabaseConfigurationBuilder();
            _typeAccessorCache = typeAccessorCache;
            _logger = logger;
        }

        public IDatabase CreateDatabase(
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
        {
            configurator.ThrowIfNull(() => configurator);

            return new Database(_logger,
                configurator.Invoke(_configurationBuilder),
                _typeAccessorCache);
        }

        public IDatabaseAsync CreateDatabaseForAsyncOperations(
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
        {
            configurator.ThrowIfNull(() => configurator);

            return new DatabaseAsync(_logger,
                configurator.Invoke(_configurationBuilder),
                _typeAccessorCache);
        }
    }
}