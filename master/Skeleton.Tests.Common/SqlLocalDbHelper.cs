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
    }
}