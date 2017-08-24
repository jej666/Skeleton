using System;
using System.Collections.Generic;

namespace Skeleton.Web.Client
{
    public class ServiceDiscoveryClient
    {
        private readonly IRestClient _client;
       
        public ServiceDiscoveryClient(Uri baseAddress)
        {
            _client = new RestClient(baseAddress);
        }

        public IEnumerable<ServiceRegistry> DiscoverServices()
        {
            var request = new RestRequest("api/discover");
            var response = _client.Get(request);

            return response.As<IEnumerable<ServiceRegistry>>();
        }
    }

    public class ServiceRegistry
    {
        public string Name { get; set; }
        public string Host { get; set; }
    }
}
