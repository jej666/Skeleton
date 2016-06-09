using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                string sql = string.Format(@"
                    CREATE DATABASE
                        [TestDb]
                    ON PRIMARY (
                       NAME=TestDb,
                       FILENAME = '{0}'
                    )", fullPath);

                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
