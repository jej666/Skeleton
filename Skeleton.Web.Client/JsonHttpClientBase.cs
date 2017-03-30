using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public abstract class JsonHttpClientBase : IDisposable
    {
        private static int Version = Assembly.GetAssembly(typeof(JsonHttpClientBase)).GetName().Version.Major;
        private readonly ExponentialRetryPolicy _retryPolicy = new ExponentialRetryPolicy();
        private readonly IRestUriBuilder _uriBuilder;
        private HttpClient _httpClient;
        private bool _disposed;

        protected JsonHttpClientBase(IRestUriBuilder uriBuilder)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            _uriBuilder = uriBuilder;
            CreateHttpClient();
        }

        public IRestUriBuilder UriBuilder => _uriBuilder;

        public HttpResponseMessage Get(Uri requestUri)
        {
            return _retryPolicy.Execute(() =>
            {
                var response = _httpClient
                    .GetAsync(requestUri)
                    .Result;

                response.HandleException();

                return response;
            });
        }

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
           {
               var response = await _httpClient
                    .GetAsync(requestUri)
                    .ConfigureAwait(false);

               response.HandleException();

               return response;
           });
        }

        public HttpResponseMessage Delete(Uri requestUri)
        {
            return _retryPolicy.Execute(() =>
            {
                var response = _httpClient
                    .DeleteAsync(requestUri)
                    .Result;

                response.HandleException();

                return response;
            });
        }

        public async Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient
                    .DeleteAsync(requestUri)
                    .ConfigureAwait(false);

                response.HandleException();

                return response;
            });
        }

        public HttpResponseMessage Put<TDto>(Uri requestUri, TDto dto) where TDto : class
        {
            return _retryPolicy.Execute(() =>
            {
                var content = CreateJsonObjectContent(dto);
                var response = _httpClient
                    .PutAsync(requestUri, content)
                    .Result;

                response.HandleException();

                return response;
            });
        }

        public async Task<HttpResponseMessage> PutAsync<TDto>(Uri requestUri, TDto dto) where TDto : class
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var content = CreateJsonObjectContent(dto);
                var response = await _httpClient
                    .PutAsync(requestUri, content)
                    .ConfigureAwait(false);

                response.HandleException();

                return response;
            });
        }

        public HttpResponseMessage Post<TDto>(Uri requestUri, TDto dto) where TDto : class
        {
            return _retryPolicy.Execute(() =>
            {
                var content = CreateJsonObjectContent(dto);
                var response = _httpClient
                    .PostAsync(requestUri, content)
                    .Result;

                response.HandleException();

                return response;
            });
        }

        public HttpResponseMessage Post<TDto>(Uri requestUri, IEnumerable<TDto> dtos) where TDto : class
        {
            return _retryPolicy.Execute(() =>
            {
                var content = CreateJsonObjectContent(dtos);
                var response = _httpClient
                    .PostAsync(requestUri, content)
                    .Result;
                response.HandleException();

                return response;
            });
        }

        public async Task<HttpResponseMessage> PostAsync<TDto>(Uri requestUri, TDto dto) where TDto : class
        {
            return await _retryPolicy.ExecuteAsync(async () =>
           {
               var content = CreateJsonObjectContent(dto);
               var response = await _httpClient
                   .PostAsync(requestUri, content)
                   .ConfigureAwait(false);

               response.HandleException();

               return response;
           });
        }

        public async Task<HttpResponseMessage> PostAsync<TDto>(Uri requestUri, IEnumerable<TDto> dtos) where TDto : class
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var content = CreateJsonObjectContent(dtos);
                var response = await _httpClient
                    .PostAsync(requestUri, content)
                    .ConfigureAwait(false);
                response.HandleException();

                return response;
            });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void CreateHttpClient()
        {
            var handler = GetCompressionHandler();
            _httpClient = new HttpClient(handler);
            SetDefaultRequestHeaders();
        }

        private static HttpClientHandler GetCompressionHandler()
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            return handler;
        }

        private void SetDefaultRequestHeaders()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(Constants.JsonMediaType));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
            _httpClient.DefaultRequestHeaders.UserAgent.Add(GetUserAgent());
        }

        private static ProductInfoHeaderValue GetUserAgent()
        {
            return new ProductInfoHeaderValue(
                new ProductHeaderValue(
                    string.Format(CultureInfo.InvariantCulture, Constants.ProductHeader, Version)));
        }

        private static ObjectContent CreateJsonObjectContent<TDto>(TDto dto) where TDto : class
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new ObjectContent<TDto>(dto, new JsonMediaTypeFormatter());
        }

        private static ObjectContent CreateJsonObjectContent<TDto>(IEnumerable<TDto> dtos) where TDto : class
        {
            if (dtos == null)
                throw new ArgumentNullException(nameof(dtos));

            return new ObjectContent<IEnumerable<TDto>>(dtos, new JsonMediaTypeFormatter());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
                return;

            if (_httpClient != null)
                _httpClient.Dispose();

            _disposed = true;
        }
    }
}