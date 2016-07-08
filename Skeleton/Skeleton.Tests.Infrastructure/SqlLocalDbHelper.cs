using System;
using System.Data.SqlClient;
using System.IO;

namespace Skeleton.Tests.Infrastructure
{
    public class SqlLocalDbHelper
    {
        private const string LocalDbPath = @"Microsoft\Microsoft SQL Server Local DB\Instances\v11.0";

        public static void CreateDatabaseIfNotExists()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var fullPath = Path.Combine(appDataPath, LocalDbPath, "testDb.mdf");

            if (File.Exists(fullPath))
                return;

            using (var connection = new SqlConnection(@"server=(localdb)\v11.0"))
            {
                connection.Open();

                var sql = string.Format(@"
                    CREATE DATABASE
                        [TestDb]
                    ON PRIMARY (
                       NAME=TestDb,
                       FILENAME = '{0}'
                    )", fullPath);

                var command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
