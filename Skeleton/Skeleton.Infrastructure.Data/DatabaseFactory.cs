namespace Skeleton.Infrastructure.Data
{
    using Common.Extensions;
    using Common.Reflection;
    using Configuration;
    using Common;
    using System;

    public sealed class DatabaseFactory : IDatabaseFactory
    {
        private readonly IDatabaseConfigurationBuilder _configurationBuilder;
        private readonly ITypeAccessorCache _typeAccessorCache;
        private readonly ILogger _logger;

        public DatabaseFactory(ITypeAccessorCache typeAccessorCache, ILogger logger)
        {
            _configurationBuilder = new DatabaseConfigurationBuilder();
            _typeAccessorCache = typeAccessorCache;
            _logger = logger;
        }

        public IDatabase CreateDatabase(Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
        {
            configurator.ThrowIfNull(() => configurator);

            return new Database(configurator.Invoke(_configurationBuilder), _typeAccessorCache, _logger);
        }

        public IDatabaseAsync CreateDatabaseForAsyncOperations(Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator)
        {
            configurator.ThrowIfNull(() => configurator);

            return new DatabaseAsync(configurator.Invoke(_configurationBuilder), _typeAccessorCache, _logger);
        }
    }
}