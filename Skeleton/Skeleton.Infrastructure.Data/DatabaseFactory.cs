using System;
using Skeleton.Infrastructure.Data.Configuration;
using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;

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

            return new Database(
                configurator.Invoke(_configurationBuilder),
                _typeAccessorCache,
                _logger);
        }

        public IDatabaseAsync CreateDatabaseForAsyncOperations(
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
        {
            configurator.ThrowIfNull(() => configurator);

            return new DatabaseAsync(
                configurator.Invoke(_configurationBuilder),
                _typeAccessorCache,
                _logger);
        }
    }
}