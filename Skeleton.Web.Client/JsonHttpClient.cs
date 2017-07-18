using System;
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
        private static Lazy<HttpClient> InnerClient;

        private readonly ExponentialRetryPolicy _retryPolicy = new ExponentialRetryPolicy();
        private readonly IRestUriBuilder _uriBuilder;
        private readonly HttpClientHandler _handler;
        private bool _disposed;

        public JsonHttpClient(IRestUriBuilder uriBuilder)
            : this(uriBuilder, new AutomaticDecompressionHandler())
        {
        }

        public JsonHttpClient(IRestUriBuilder uriBuilder, HttpClientHandler handler)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            _uriBuilder = uriBuilder;
            _handler = handler;

            InnerClient = new Lazy<HttpClient>(
                () => new HttpClient(_handler));
        }

        public IRestUriBuilder UriBuilder => _uriBuilder;

        public AuthenticationHeaderValue Authentication { get; set; }

        public HttpClient Client => InnerClient.Value;

        public HttpResponseMessage Send(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            CheckDisposed();
            SetAuthentication(request);

            return _retryPolicy.Execute(() =>
            {
                var response = Client
                    .SendAsync(request)
                    .Result;

                response.HandleException();

                return response;
            });
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            CheckDisposed();
            SetAuthentication(request);

            return await _retryPolicy.Execute(async () =>
            {
                var response = await Client
                    .SendAsync(request)
                    .ConfigureAwait(false);

                response.HandleException();

                return response;
            });
        }

        public HttpResponseMessage Get(Uri requestUri)
        {
            return Send(new HttpRequestMessage(HttpMethod.Get, requestUri));
        }

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return await SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri));
        }

        public HttpResponseMessage Delete(Uri requestUri)
        {
            return Send(new HttpRequestMessage(HttpMethod.Delete, requestUri));
        }

        public async Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            return await SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri));
        }

        public HttpResponseMessage Put<T>(Uri requestUri, T value)
        {
            var content = new JsonObjectContent(value);
            var request = new HttpRequestMessage(HttpMethod.Put, requestUri) { Content = content };

            return Send(request);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(Uri requestUri, T value)
        {
            var content = new JsonObjectContent(value);
            var request = new HttpRequestMessage(HttpMethod.Put, requestUri) { Content = content };

            return await SendAsync(request);
        }

        public HttpResponseMessage Post<T>(Uri requestUri, T value)
        {
            var content = new JsonObjectContent(value);
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = content };

            return Send(request);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(Uri requestUri, T value)
        {
            var content = new JsonObjectContent(value);
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = content };

            return await SendAsync(request);
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

        private void SetAuthentication(HttpRequestMessage request)
        {
            if (Authentication != null)
                request.Headers.Authorization = Authentication;
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
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