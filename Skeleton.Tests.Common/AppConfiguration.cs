using System;

namespace Skeleton.Tests.Common
{
    public static class AppConfiguration
    {
        private const string CustomersPath = "api/customers";
        private const string CachedCustomersPath = "api/cachedcustomers";
        private const string AsyncCustomersPath = "api/asynccustomers";
        private const string AsyncCachedCustomersPath = "api/asynccachedcustomers";

        public static bool AppVeyorBuild => Environment.GetEnvironmentVariable("AppVeyor")?.ToUpperInvariant() == "TRUE";

        public static string ConnectionString => AppVeyorBuild
            ? @"Server=(local)\SQL2014;Database=TestDb;User ID = sa; Password=Password12!;"
            : @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True;";

        public static string Host => "localhost";
        public static int Port => 8081;
        public static UriBuilder CustomersUriBuilder => new UriBuilder("http", Host, Port, CustomersPath);
        public static UriBuilder CachedCustomersUriBuilder => new UriBuilder("http", Host, Port, CachedCustomersPath);
        public static UriBuilder AsyncCustomersUriBuilder => new UriBuilder("http", Host, Port, AsyncCustomersPath);
        public static UriBuilder AsyncCachedCustomersUriBuilder => new UriBuilder("http", Host, Port, AsyncCachedCustomersPath);
        public static Uri BaseUrl => new UriBuilder("http", Host, Port).Uri;
    }
}