using Skeleton.Abstraction.Dependency;
using Skeleton.Infrastructure.Dependency;
using Skeleton.Tests.Common;
using System;

namespace Skeleton.Documentation.Performance
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Skeleton Performance Runs");
            Console.WriteLine("____________________________________________________");

            SqlLocalDbHelper.CreateDatabaseIfNotExists();

            var bootstrapper = new Bootstrapper();
            bootstrapper.UseSqlServer(
                            builder => builder.UsingConnectionString(AppConfiguration.ConnectionString).Build())
                        .WithOrm();

            Console.WriteLine("Skeleton Orm Performance Run => ");
            Console.WriteLine();

            var ormPerformance = new OrmPerformance(bootstrapper.Container as IDependencyResolver);
            ormPerformance.RunBenchmarks();

            Console.WriteLine("_____________________________________________________");
            Console.WriteLine("Skeleton MetaData Performance Run =>");
            Console.WriteLine();

            var metaDataPerformance = new MetadataPerformance(bootstrapper.Container as IDependencyResolver);
            metaDataPerformance.RunBenchmarks();

            Console.WriteLine("_____________________________________________________");
            Console.ReadKey();
        }
    }
}