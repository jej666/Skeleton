using System;
using System.Data.SqlClient;
using System.IO;

namespace Skeleton.Tests.Common
{
    public static class SqlLocalDbHelper
    {
        private const string LocalDbPath = @"Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB";

        public static void CreateDatabaseIfNotExists()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var fullPath = Path.Combine(appDataPath, LocalDbPath, "testDb.mdf");

            if (File.Exists(fullPath))
                return;

            using (var connection = new SqlConnection(@"server=(localdb)\MSSQLLocalDB"))
            {
                connection.Open();

                var sql = $@"
                    CREATE DATABASE
                        [TestDb]
                    ON PRIMARY (
                       NAME=TestDb,
                       FILENAME = '{fullPath}')";

                var command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        public static void InstallProcStocIfNotExists()
        {
            var connection = new SqlConnectionHelper();
            using (var cnn = connection.OpenConnection())
            {
                var cmd = cnn.CreateCommand();
                cmd.CommandText = @"
                    IF OBJECT_ID('ProcedureSelectCustomerByCategory', 'P') IS NOT NULL
                        DROP PROCEDURE[dbo].[ProcedureSelectCustomerByCategory]
                    GO

                    CREATE PROCEDURE[dbo].[ProcedureSelectCustomerByCategory]
                        @categoryId int
                    AS
                        BEGIN
                            SELECT* FROM[dbo].[Customer]
                            WHERE[dbo].Customer.CustomerCategoryId = @categoryId
                        END
                    GO";
                cmd.Connection = cnn;
                cmd.ExecuteNonQuery();
            }
        }
    }
}