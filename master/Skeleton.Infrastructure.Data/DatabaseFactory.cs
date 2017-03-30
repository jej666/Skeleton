using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Reflection;
using Skeleton.Common;
using System;

namespace Skeleton.Infrastructure.Data
{
    public sealed class DatabaseFactory : IDatabaseFactory
    {
        private readonly IDatabaseConfigurationBuilder _configurationBuilder;
        private readonly ILogger _logger;
        private readonly IMetadataProvider _metadataProvider;

        public DatabaseFactory(IMetadataProvider metadataProvider, ILogger logger)
        {
            _configurationBuilder = new DatabaseConfigurationBuilder();
            _metadataProvider = metadataProvider;
            _logger = logger;
        }

        public IDatabase CreateDatabase(
                Func<IDatabaseConfigurationBuilder,
                IDatabaseConfiguration> configurator)
        {
            configurator.ThrowIfNull(nameof(configurator));

            return new Database(_logger,
                configurator.Invoke(_configurationBuilder),
                _metadataProvider);
        }

        public IAsyncDatabase CreateDatabaseForAsyncOperations(
                Func<IDatabaseConfigurationBuilder,
                IDatabaseConfiguration> configurator)
        {
            configurator.ThrowIfNull(nameof(configurator));

            return new AsyncDatabase(_logger,
                configurator.Invoke(_configurationBuilder),
                _metadataProvider);
        }
    }
}