using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;

namespace Skeleton.Web.Client
{
    public class ServiceDiscoveryClient
    {
        private readonly JsonHttpClient _client;
        private readonly RestUriBuilder _uriBuilder;
       
        public ServiceDiscoveryClient(Uri baseAddress)
        {
            _uriBuilder = new RestUriBuilder(new UriBuilder(baseAddress));
            _client = new JsonHttpClient(_uriBuilder);
        }

        public IEnumerable<ServiceRegistry> DiscoverServices()
        {
            _uriBuilder.AppendAction("api/discover");
            var response = _client.Get(_uriBuilder.Uri);

            return response
                .Content
                .ReadAsAsync<IEnumerable<ServiceRegistry>>()
                .Result;
        }
    }

    public class ServiceRegistry
    {
        public string Name { get; set; }
        public string Host { get; set; }
    }
}
