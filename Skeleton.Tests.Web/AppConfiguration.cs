using System;
using System.Configuration;

namespace Skeleton.Tests.Web
{
    public static class AppConfiguration
    {
        private static string CustomersPath => GetAppSetting("CustomersPath");
        private static string CachedCustomersPath => GetAppSetting("CachedCustomersPath");
        private static string AsyncCustomersPath => GetAppSetting("AsyncCustomersPath");
        private static string AsyncCachedCustomersPath => GetAppSetting("AsyncCachedCustomersPath");
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
    }
}