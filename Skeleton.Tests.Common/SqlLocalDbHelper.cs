using System;
using System.Data.SqlClient;
using System.IO;

namespace Skeleton.Tests.Common
{
    public static class SqlLocalDbHelper
    {
        private const string LocalDbPath = @"Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB";
        private const string LocalDbName = "testDb";
        private const string Mdf = ".mdf";

        public static void CreateDatabaseIfNotExists()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var fullPath = Path.Combine(appDataPath, LocalDbPath, LocalDbName + Mdf);

            //if (File.Exists(fullPath))
            //    return;

            var connection = new SqlConnectionHelper();
            using (var cnn = connection.OpenConnection())
            {
                var sql = $@"
                    If Not Exists(Select * from sys.databases Where name = '{LocalDbName}')
                    Begin
                        CREATE DATABASE
                            [TestDb]
                        ON PRIMARY (
                           NAME={LocalDbName},
                           FILENAME = '{fullPath}')
                    End";

                using (var command = new SqlCommand(sql, cnn))
                    command.ExecuteNonQuery();
            }
        }

        public static void InstallProcStocIfNotExists()
        {
            using (var connection = new SqlConnectionHelper())
            using (var cnn = connection.OpenConnection())
            {
                var cmd = cnn.CreateCommand();
                cmd.Connection = cnn;
                cmd.CommandText = @"
                    IF OBJECT_ID('ProcedureSelectCustomerByCategory', 'P') IS NOT NULL
                        DROP PROCEDURE[dbo].[ProcedureSelectCustomerByCategory]";

                cmd.ExecuteNonQuery();
                cmd.CommandText = @"
                CREATE PROCEDURE[dbo].[ProcedureSelectCustomerByCategory]
                    @categoryId int
                AS
                   BEGIN
                     SELECT* FROM[dbo].[Customer]
                     WHERE[dbo].Customer.CustomerCategoryId = @categoryId
                   END";
                cmd.ExecuteNonQuery();
            }
        }
    }
}