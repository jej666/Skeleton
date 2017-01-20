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
        private readonly string _serviceAddress;
        private bool _disposed;
        private HttpClient _httpClient;

        protected HttpClientBase(string serviceBaseAddress, string addressSuffix)
        {
            if (!addressSuffix.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                addressSuffix += "/";

            _serviceAddress = serviceBaseAddress + addressSuffix;
            CreateJsonHttpClient();
        }

        protected HttpClient JsonHttpClient
        {
            get { return _httpClient; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Uri CreateUri(string action)
        {
            return new Uri(_serviceAddress + action);
        }

        private  void CreateJsonHttpClient()
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(JsonMediaType));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
            _httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(new ProductHeaderValue("Skeleton_HttpClient", "1.0")));
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