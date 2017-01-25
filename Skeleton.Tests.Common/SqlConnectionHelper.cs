using System;
using System.Data.SqlClient;

namespace Skeleton.Tests.Common
{
    public sealed class SqlConnectionHelper : IDisposable
    {
        private const string ConnectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True;Pooling=False;";

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