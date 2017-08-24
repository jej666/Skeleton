using Skeleton.Web.Client;
using System;

namespace Skeleton.Tests.Common
{
    public static class AppConfiguration
    {
        public const string CustomersResource = "customers";
        public const string CachedCustomersResource = "cachedcustomers";
        public const string AsyncCustomersResource = "asynccustomers";
        public const string AsyncCachedCustomersResource = "asynccachedcustomers";
        public const string LogResource = "log";

        public static bool AppVeyorBuild => Environment.GetEnvironmentVariable("AppVeyor")?.ToUpperInvariant() == "TRUE";

        public static string ConnectionString => AppVeyorBuild
            ? @"Server=(local)\SQL2014;Database=TestDb;User ID = sa; Password=Password12!;"
            : @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True;";

        public static string Host => "localhost";
        public static int Port => 8081;
        public static Uri BaseAddress => new UriBuilder("http", Host, Port).Uri;
    }
}