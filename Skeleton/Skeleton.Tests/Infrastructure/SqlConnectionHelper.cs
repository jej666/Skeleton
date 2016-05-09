using System;
using System.Data.SqlClient;

namespace Skeleton.Tests.Infrastructure
{
    public sealed class SqlConnectionHelper : IDisposable
    {
        private static string ConnectionString = @"Data Source=(localdb)\v11.0;Initial Catalog=TestDb;Integrated Security=True;Pooling=False;";
        private readonly SqlConnection innerConnection = new SqlConnection(ConnectionString);

        public void CloseConnection()
        {
            if (innerConnection != null)
            {
                innerConnection.Close();
                innerConnection.Dispose();
            }
        }

        public void Dispose()
        {
            CloseConnection();
        }

        public SqlConnection OpenConnection()
        {
            innerConnection.Open();

            return innerConnection;
        }
    }
}