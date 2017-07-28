using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Skeleton.Web.Client
{
    public static class ClientProvider
    {
        private static readonly ConcurrentDictionary<string, JsonHttpClient> ClientCache =
           new ConcurrentDictionary<string, JsonHttpClient>();

        public static IEnumerable<string> Keys => ClientCache.Keys;

        public static IEnumerable<JsonHttpClient> Values => ClientCache.Values;

        public static void RegisterServices(Uri baseAddress)
        {
            var client = new ServiceDiscoveryClient(baseAddress);
            var serviceRegistry = client.DiscoverServices();
            
            foreach (var service in serviceRegistry)
            {
                var serviceUri = new RestUriBuilder(new UriBuilder(service.Host));
                var clientInstance = new JsonHttpClient(serviceUri);

                ClientCache.TryAdd(service.Name.ToLower(), clientInstance);
            }
        }

        public static JsonHttpClient FindClient(string key)
        {
            JsonHttpClient value = null;
            return ClientCache.TryGetValue(key.ToLower(), out value) ? value : null;
        }
    }
}
