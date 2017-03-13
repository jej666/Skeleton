using Skeleton.Abstraction.Data;
using Skeleton.Common;
using System.Configuration;

namespace Skeleton.Infrastructure.Data
{
    public sealed class DatabaseConfigurationBuilder :
        HideObjectMethodsBase,
        IDatabaseConfigurationBuilder,
        IDatabaseConfigurationProperties,
        IDatabaseConfigurationSettings,
        IDatabaseConfigurationRetryPolicy,
        IDatabaseConfigurationRetryPolicyEnd
    {
        private const string NoConnectionStringInConfigFile = "No connection settings found in config file";
        private readonly DatabaseConfiguration _configuration = new DatabaseConfiguration();

        public IDatabaseConfigurationProperties UsingConfigConnectionString(string connectionStringConfigName)
        {
            var connectionSettings = ConfigurationManager.ConnectionStrings;

            if (connectionSettings?[connectionStringConfigName] == null)
                throw new ConfigurationErrorsException(NoConnectionStringInConfigFile);

            var namedDatabase = connectionSettings[connectionStringConfigName];
            Initialize(namedDatabase);

            return this;
        }

        public IDatabaseConfigurationProperties UsingConnectionString(string connectionString)
        {
            connectionString.ThrowIfNullOrEmpty(() => connectionString);

            var settings = new ConnectionStringSettings { ConnectionString = connectionString };
            Initialize(settings);

            return this;
        }

        public IDatabaseConfigurationProperties UsingDefaultConfigConnectionString()
        {
            var connectionSettings = ConfigurationManager.ConnectionStrings;

            if (connectionSettings?[0] == null)
                throw new ConfigurationErrorsException(NoConnectionStringInConfigFile);

            var defaultDatabase = connectionSettings[0];
            Initialize(defaultDatabase);

            return this;
        }

        public IDatabaseConfiguration Build()
        {
            return _configuration;
        }

        public IDatabaseConfigurationSettings UsingAdvancedSettings()
        {
            return this;
        }

        public IDatabaseConfigurationRetryPolicyEnd SetRetryPolicyCount(int value)
        {
            _configuration.RetryCount = value;

            return this;
        }

        public IDatabaseConfiguration SetRetryPolicyInterval(int value)
        {
            _configuration.RetryInterval = value;

            return _configuration;
        }

        public IDatabaseConfigurationRetryPolicy SetCommandTimeout(int seconds)
        {
            _configuration.CommandTimeout = seconds;

            return this;
        }

        private void Initialize(ConnectionStringSettings settings)
        {
            _configuration.ConnectionString = settings.ConnectionString;
            _configuration.Name = settings.Name;
            _configuration.ProviderName = !string.IsNullOrEmpty(settings.ProviderName)
                ? settings.ProviderName
                : "System.Data.SqlClient";
        }
    }
}