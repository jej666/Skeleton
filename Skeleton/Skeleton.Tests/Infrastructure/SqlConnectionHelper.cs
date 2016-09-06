using System;
using System.Data.SqlClient;

namespace Skeleton.Tests.Infrastructure
{
    public sealed class SqlConnectionHelper : IDisposable
    {
        private const string ConnectionString =
            @"Data Source=(localdb)\v11.0;Initial Catalog=TestDb;Integrated Security=True;Pooling=False;";

        private readonly SqlConnection _innerConnection = new SqlConnection(ConnectionString);

        public void Dispose()
        {
            CloseConnection();
        }

        private void CloseConnection()
        {
            if (_innerConnection == null) return;

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