using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Skeleton.Web.Client
{
    public abstract class HttpClientBase<TDto> :
        IDisposable where TDto : class
    {
        private readonly string _addressSuffix;
        private readonly string _jsonMediaType = "application/json";
        private readonly string _serviceBaseAddress;
        private bool _disposed;
        private HttpClient _httpClient;

        protected HttpClientBase(string serviceBaseAddress, string addressSuffix)
        {
            _serviceBaseAddress = serviceBaseAddress;
            _addressSuffix = addressSuffix;

            if (!_addressSuffix.EndsWith("/"))
                _addressSuffix += "/";
        }

        public HttpClient JsonHttpClient
        {
            get
            {
                return _httpClient ??
                       (_httpClient = CreateJsonHttpClient(_serviceBaseAddress));
            }
        }

        public string AddressSuffix
        {
            get { return _addressSuffix; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private HttpClient CreateJsonHttpClient(string serviceBaseAddress)
        {
            var client = new HttpClient {BaseAddress = new Uri(serviceBaseAddress)};

            client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(_jsonMediaType));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
            client.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(new ProductHeaderValue("Skeleton_HttpClient", "1.0")));

            return client;
        }

        protected virtual ObjectContent CreateJsonObjectContent(TDto dto)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            return new ObjectContent<TDto>(dto, jsonFormatter);
        }

        protected virtual ObjectContent CreateJsonObjectContent(IEnumerable<TDto> dtos)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            return new ObjectContent<IEnumerable<TDto>>(dtos, jsonFormatter);
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