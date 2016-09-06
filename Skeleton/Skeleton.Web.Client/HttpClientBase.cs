using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Skeleton.Web.Client
{
    public abstract class HttpClientBase :
        IDisposable 
    {
        private const string JsonMediaType = "application/json";
        private readonly string _serviceBaseAddress;
        private bool _disposed;
        private HttpClient _httpClient;

        protected HttpClientBase(string serviceBaseAddress, string addressSuffix)
        {
            _serviceBaseAddress = serviceBaseAddress;
            AddressSuffix = addressSuffix;

            if (!AddressSuffix.EndsWith("/"))
                AddressSuffix += "/";
        }

        protected HttpClient JsonHttpClient => _httpClient ??
                                            (_httpClient = CreateJsonHttpClient(_serviceBaseAddress));

        protected string AddressSuffix { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static HttpClient CreateJsonHttpClient(string serviceBaseAddress)
        {
            var client = new HttpClient {BaseAddress = new Uri(serviceBaseAddress)};

            client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(JsonMediaType));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
            client.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(new ProductHeaderValue("Skeleton_HttpClient", "1.0")));

            return client;
        }

        protected virtual ObjectContent CreateJsonObjectContent<TDto>(TDto dto) where TDto : class
        {
            return new ObjectContent<TDto>(dto, new JsonMediaTypeFormatter());
        }

        protected virtual ObjectContent CreateJsonObjectContent<TDto>(IEnumerable<TDto> dtos) where TDto : class
        {
            return new ObjectContent<IEnumerable<TDto>>(dtos, new JsonMediaTypeFormatter());
        }

        private void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
                return;

            if (_httpClient != null)
            {
                var hc = _httpClient;
                _httpClient = null;
                hc.Dispose();
            }
            _disposed = true;
        }
    }
}