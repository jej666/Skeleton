using System;
using System.Data.SqlClient;

namespace Skeleton.Tests.Common
{
    public sealed class SqlConnectionHelper : IDisposable
    {
        private readonly SqlConnection _innerConnection = 
            new SqlConnection(AppConfiguration.ConnectionString);

        public void Dispose()
        {
            CloseConnection();
        }

        private void CloseConnection()
        {
            if (_innerConnection == null)
                return;

            _innerConnection.Close();
            _innerConnection.Dispose();
        }

        public SqlConnection OpenConnection()
        {
            _innerConnection.Open();

            return _innerConnection;
        }
    }
}