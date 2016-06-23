using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Skeleton.Web.Client
{
    public abstract class HttpClientBase<TEntity, TIdentity> :
        IDisposable where TEntity : class
    {
        private bool disposed = false;
        private HttpClient _httpClient;
        private readonly string _serviceBaseAddress;
        private readonly string _addressSuffix;
        private readonly string _jsonMediaType = "application/json";

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
                if (_httpClient == null)
                    _httpClient = CreateJsonHttpClient(_serviceBaseAddress);

                return _httpClient;
            }
        }

        public string AddressSuffix
        {
            get { return _addressSuffix; }
        }

        private HttpClient CreateJsonHttpClient(string serviceBaseAddress)
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(serviceBaseAddress);
            client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(_jsonMediaType));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("Skeleton_HttpClient", "1.0")));

            return client;
        }

        protected virtual ObjectContent CreateJsonObjectContent(TEntity model)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            return new ObjectContent<TEntity>(model, jsonFormatter);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                if (_httpClient != null)
                {
                    var hc = _httpClient;
                    _httpClient = null;
                    hc.Dispose();
                }
                disposed = true;
            }
        }
    }
}
