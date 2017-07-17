using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public class JsonHttpClient : IDisposable
    {
        private static int Version = Assembly.GetAssembly(typeof(JsonHttpClient)).GetName().Version.Major;
        private static Lazy<HttpClient> HttpClient;

        private readonly ExponentialRetryPolicy _retryPolicy = new ExponentialRetryPolicy();
        private readonly IRestUriBuilder _uriBuilder;
        private readonly HttpClientHandler _handler;
        private bool _disposed;
      
        public JsonHttpClient(IRestUriBuilder uriBuilder, HttpClientHandler handler)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            _uriBuilder = uriBuilder;
            _handler = handler as HttpClientHandler;

            HttpClient = new Lazy<HttpClient>(
                () => new HttpClient(_handler));
        }

        public IRestUriBuilder UriBuilder => _uriBuilder;

        protected HttpClient Client => HttpClient.Value;

        public HttpResponseMessage Get(Uri requestUri)
        {
            return _retryPolicy.Execute(() =>
            {
                var response = Client
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
               var response = await Client
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
                var response = Client
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
                var response = await Client
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
                var content =  new JsonObjectContent(dto);
                var response = Client
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
                var content = new JsonObjectContent(dto);
                var response = await Client
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
                var content = new JsonObjectContent(dto);
                var response = Client
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
                var content = new JsonObjectContent(dtos);
                var response = Client
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
               var content = new JsonObjectContent(dto);
               var response = await Client
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
                var content = new JsonObjectContent(dtos);
                var response = await Client
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

        private void SetDefaultRequestHeaders()
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(Constants.JsonMediaType));
            Client.DefaultRequestHeaders.UserAgent.Add(GetUserAgent());

            if (_handler.SupportsAutomaticDecompression)
            {
                Client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
                Client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
            }
        }

        private static ProductInfoHeaderValue GetUserAgent()
        {
            return new ProductInfoHeaderValue(
                new ProductHeaderValue(
                    string.Format(CultureInfo.InvariantCulture, Constants.ProductHeader, Version)));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
                return;

            Client?.Dispose();

            _disposed = true;
        }
    }
}