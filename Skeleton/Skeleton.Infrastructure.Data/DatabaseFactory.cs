using System;
using Skeleton.Common;
using Skeleton.Common.Extensions;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data.Configuration;

namespace Skeleton.Infrastructure.Data
{
    public sealed class DatabaseFactory : IDatabaseFactory
    {
        private readonly IDatabaseConfigurationBuilder _configurationBuilder;
        private readonly ILogger _logger;
        private readonly ITypeAccessorCache _typeAccessorCache;

        public DatabaseFactory(ITypeAccessorCache typeAccessorCache, ILogger logger)
        {
            _configurationBuilder = new DatabaseConfigurationBuilder();
            _typeAccessorCache = typeAccessorCache;
            _logger = logger;
        }

        public IDatabase CreateDatabase(Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
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