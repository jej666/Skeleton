using System;
using System.Data.SqlClient;
using System.IO;

namespace Skeleton.Tests.Common
{
    public static class SqlLocalDbHelper
    {
        private const string LocalDbPath = @"Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB";
        private const string DbName = "TestDb";
        private const string MdfExtension = ".mdf";
        private const string LocalServer = @"server=(localdb)\MSSQLLocalDB";

        public static void CreateDatabaseIfNotExists()
        {
            if (AppConfiguration.AppVeyorBuild)
                return;

            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var fullPath = Path.Combine(appDataPath, LocalDbPath, DbName + MdfExtension);

            if (File.Exists(fullPath))
                return;

            CreateDb(fullPath, LocalServer);
        }

        private static void CreateDb(string fullPath, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var sql = $@"
                    CREATE DATABASE
                        [{DbName}]
                    ON PRIMARY
                        (
                            NAME={DbName},
                            FILENAME = '{fullPath}'
                        )";

                using (var command = new SqlCommand(sql, connection))
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