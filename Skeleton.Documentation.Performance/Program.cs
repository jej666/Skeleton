using Skeleton.Infrastructure.DependencyInjection;
using Skeleton.Tests.Common;
using System;

namespace Skeleton.Documentation.Performance
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Skeleton Performance Runs");
            Console.WriteLine("____________________________________________________");

            SqlLocalDbHelper.CreateDatabaseIfNotExists();

            Bootstrapper.UseDatabase(builder =>
                    builder.UsingConnectionString(AppConfiguration.ConnectionString)
                    .Build());

            Console.WriteLine("Skeleton Orm Performance Run => ");
            Console.WriteLine();

            var ormPerformance = new OrmPerformance();
            ormPerformance.RunBenchmarks();

            Console.WriteLine("_____________________________________________________");
            Console.WriteLine("Skeleton MetaData Performance Run =>");
            Console.WriteLine();

            var metaDataPerformance = new MetadataPerformance();
            metaDataPerformance.RunBenchmarks();

            Console.WriteLine("_____________________________________________________");
            Console.ReadKey();
        }
    }
}
