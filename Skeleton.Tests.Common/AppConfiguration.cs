using System;

namespace Skeleton.Tests.Common
{
    public static class AppConfiguration
    {
        public const string CustomersResource = "api/customers";
        public const string CachedCustomersResource = "api/cachedcustomers";
        public const string AsyncCustomersResource = "api/asynccustomers";
        public const string AsyncCachedCustomersResource = "api/asynccachedcustomers";

        public static bool AppVeyorBuild => Environment.GetEnvironmentVariable("AppVeyor")?.ToUpperInvariant() == "TRUE";

        public static string ConnectionString => AppVeyorBuild
            ? @"Server=(local)\SQL2014;Database=TestDb;User ID = sa; Password=Password12!;"
            : @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True;";

        public static string Host => "localhost";
        public static int Port => 8081;
        public static Uri BaseAddress => new UriBuilder("http", Host, Port).Uri;
        public static Uri CustomersUri => new Uri(BaseAddress, new Uri(CustomersResource, UriKind.Relative));
        public static Uri CachedCustomersUri => new Uri(BaseAddress, new Uri(CachedCustomersResource, UriKind.Relative));
        public static Uri AsyncCustomersUri => new Uri(BaseAddress, new Uri(AsyncCustomersResource, UriKind.Relative));
        public static Uri AsyncCachedCustomersUri => new Uri(BaseAddress, new Uri(AsyncCachedCustomersResource, UriKind.Relative));
    }
}