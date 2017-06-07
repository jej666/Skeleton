using Skeleton.Abstraction;
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

            var bootstrapper = new Bootstrapper();
            bootstrapper.Builder
                        .UseSqlServer(
                            builder => builder.UsingConnectionString(AppConfiguration.ConnectionString).Build())
                        .WithOrm();

            Console.WriteLine("Skeleton Orm Performance Run => ");
            Console.WriteLine();

            var ormPerformance = new OrmPerformance(bootstrapper as IDependencyResolver);
            ormPerformance.RunBenchmarks();

            Console.WriteLine("_____________________________________________________");
            Console.WriteLine("Skeleton MetaData Performance Run =>");
            Console.WriteLine();

            var metaDataPerformance = new MetadataPerformance(bootstrapper as IDependencyResolver);
            metaDataPerformance.RunBenchmarks();

            Console.WriteLine("_____________________________________________________");
            Console.ReadKey();
        }
    }
}
