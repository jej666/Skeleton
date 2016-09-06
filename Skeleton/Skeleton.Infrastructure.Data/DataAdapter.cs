using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Skeleton.Abstraction.Data;

namespace Skeleton.Infrastructure.Data
{
    internal sealed class DataAdapter
    {
        private readonly IDatabaseConfiguration _configuration;

        internal DataAdapter(IDatabaseConfiguration configuration)
        {
            _configuration = configuration;
            ProviderType = GetProviderType(configuration.ProviderName);
        }

        internal DataProviderType ProviderType { get; }

        internal IDbCommand CreateCommand()
        {
            switch (ProviderType)
            {
                case DataProviderType.SqlServer:
                    return new SqlCommand();

                case DataProviderType.OleDb:
                    return new OleDbCommand();

                case DataProviderType.Odbc:
                    return new OdbcCommand();

                default:
                    return null;
            }
        }

        internal IDbConnection CreateConnection()
        {
            switch (ProviderType)
            {
                case DataProviderType.SqlServer:
                    return new SqlConnection(_configuration.ConnectionString);

                case DataProviderType.OleDb:
                    return new OleDbConnection(_configuration.ConnectionString);

                case DataProviderType.Odbc:
                    return new OdbcConnection(_configuration.ConnectionString);

                default:
                    return null;
            }
        }

        internal IDataParameter CreateParameter()
        {
            switch (ProviderType)
            {
                case DataProviderType.SqlServer:
                    return new SqlParameter();

                case DataProviderType.OleDb:
                    return new OleDbParameter();

                case DataProviderType.Odbc:
                    return new OdbcParameter();

                default:
                    return null;
            }
        }

        private static DataProviderType GetProviderType(string providerName)
        {
            switch (providerName)
            {
                case "System.Data.SqlClient":
                    return DataProviderType.SqlServer;

                case "System.Data.OleDb":
                    return DataProviderType.OleDb;

                case "System.Data.Odbc":
                    return DataProviderType.Odbc;

                default:
                    return DataProviderType.SqlServer;
            }
        }
    }
}