using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public abstract class HttpClientBase : IDisposable
    {
        private const string JsonMediaType = "application/json";
        private const string ProductHeader = "SkeletonHttpClient";
        private readonly IRestUriBuilder _uriBuilder;
        private HttpClient _httpClient;
        private bool _disposed;

        protected HttpClientBase(IRestUriBuilder uriBuilder)
        {
            _uriBuilder = uriBuilder;
            CreateHttpClient();
        }

        protected IRestUriBuilder UriBuilder => _uriBuilder;

        protected HttpResponseMessage Get(Uri requestUri)
        {
            var response = _httpClient.GetAsync(requestUri).Result;
            response.EnsureSuccessStatusCode();

            return response;
        }

        protected async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            return response;
        }

        protected HttpResponseMessage Post<TDto>(Uri requestUri, TDto dto) where TDto : class
        {
            var content = CreateJsonObjectContent(dto);
            var response = _httpClient.PostAsync(requestUri, content).Result;
            response.EnsureSuccessStatusCode();

            return response;
        }

        protected HttpResponseMessage Post<TDto>(Uri requestUri, IEnumerable<TDto> dtos) where TDto : class
        {
            var content = CreateJsonObjectContent(dtos);
            var response = _httpClient.PostAsync(requestUri, content).Result;
            response.EnsureSuccessStatusCode();

            return response;
        }

        protected async Task<HttpResponseMessage> PostAsync<TDto>(Uri requestUri, TDto dto) where TDto : class
        {
            var content = CreateJsonObjectContent(dto);
            var response = await _httpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();

            return response;
        }

        protected async Task<HttpResponseMessage> PostAsync<TDto>(Uri requestUri, IEnumerable<TDto> dtos) where TDto : class
        {
            var content = CreateJsonObjectContent(dtos);
            var response = await _httpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void CreateHttpClient()
        {
            using (var handler = new HttpClientHandler())
            {
                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }
                _httpClient = new HttpClient(handler);

                _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(JsonMediaType));
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
                _httpClient.DefaultRequestHeaders.UserAgent.Add(
                    new ProductInfoHeaderValue(new ProductHeaderValue(ProductHeader, "1.0")));
            }
        }

        private ObjectContent CreateJsonObjectContent<TDto>(TDto dto) where TDto : class
        {
            return new ObjectContent<TDto>(dto, new JsonMediaTypeFormatter());
        }

        private ObjectContent CreateJsonObjectContent<TDto>(IEnumerable<TDto> dtos) where TDto : class
        {
            return new ObjectContent<IEnumerable<TDto>>(dtos, new JsonMediaTypeFormatter());
        }

        protected virtual void Dispose(bool disposing)
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