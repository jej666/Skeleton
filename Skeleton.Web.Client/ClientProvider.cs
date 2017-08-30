using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Skeleton.Web.Client
{
    public static class ClientProvider
    {
        private static readonly ConcurrentDictionary<string, IRestClient> ClientCache =
           new ConcurrentDictionary<string, IRestClient>();

        public static IEnumerable<string> Keys => ClientCache.Keys;

        public static IEnumerable<IRestClient> Values => ClientCache.Values;

        public static void RegisterServices(Uri baseAddress)
        {
            var client = new ServiceDiscoveryClient(baseAddress);
            var serviceRegistry = client.DiscoverServices();
            
            foreach (var service in serviceRegistry)
            {
                var serviceUri = new Uri(service.Host);
                var clientInstance = new RestClient(serviceUri);

                ClientCache.TryAdd(service.Name.ToLower(), clientInstance);
            }
        }

        public static IRestClient GetClient(string key)
        {
            EnsureKeyNotNullOrEmpty(key);

            IRestClient value = null;
            return ClientCache.TryGetValue(key.ToLower(), out value) ? value : null;
        }

        public static bool RemoveClient(string key)
        {
            EnsureKeyNotNullOrEmpty(key);

            IRestClient value = null;
            return ClientCache.TryRemove(key.ToLower(), out value);
        }

        private static void EnsureKeyNotNullOrEmpty(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
        }
    }
}
