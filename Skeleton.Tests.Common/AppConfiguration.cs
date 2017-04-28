using System;
using System.Configuration;

namespace Skeleton.Tests.Common
{
    public static class AppConfiguration
    {
        private static readonly string CustomersPath = GetAppSetting("CustomersPath");
        private static readonly string CachedCustomersPath = GetAppSetting("CachedCustomersPath");
        private static readonly string AsyncCustomersPath = GetAppSetting("AsyncCustomersPath");
        private static readonly string AsyncCachedCustomersPath = GetAppSetting("AsyncCachedCustomersPath");

        public static bool AppVeyorBuild => Environment.GetEnvironmentVariable("AppVeyor")?.ToUpperInvariant() == "TRUE";
        public static string ConnectionString => AppVeyorBuild 
            ? GetConnectionString("AppVeyor")
            : GetConnectionString("Default");

        public static string Host => GetAppSetting("Host");
        public static int Port => int.Parse(GetAppSetting("Port"));
        public static UriBuilder CustomersUriBuilder => new UriBuilder("http", Host, Port, CustomersPath);
        public static UriBuilder CachedCustomersUriBuilder => new UriBuilder("http", Host, Port, CachedCustomersPath);
        public static UriBuilder AsyncCustomersUriBuilder => new UriBuilder("http", Host, Port, AsyncCustomersPath);
        public static UriBuilder AsyncCachedCustomersUriBuilder => new UriBuilder("http", Host, Port, AsyncCachedCustomersPath);
        public static Uri BaseUrl => new UriBuilder("http", Host, Port).Uri;

        private static string GetAppSetting(string key)
        {
            var appSetting = ConfigurationManager.AppSettings[key];
            if (appSetting == null)
                throw new ApplicationException($"appSetting {key} must be set.");

            return appSetting;
        }

        private static string GetConnectionString(string connectionStringConfigName)
        {
            var connectionSettings = ConfigurationManager.ConnectionStrings;

            if (connectionSettings?[connectionStringConfigName] == null)
                throw new ConfigurationErrorsException($"connectionString {connectionStringConfigName} must be set.");

            var namedDatabase = connectionSettings[connectionStringConfigName];

            return namedDatabase.ConnectionString;
        }
    }
}