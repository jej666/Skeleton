using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public class JsonHttpClient : IDisposable
    {
        private static int Version = Assembly.GetAssembly(typeof(JsonHttpClient)).GetName().Version.Major;
        private static Lazy<HttpClient> InnerClient;

        private readonly JsonMediaTypeFormatter _jsonFormatter = new JsonMediaTypeFormatter();
        private readonly IRetryPolicy _retryPolicy;
        private readonly IRestUriBuilder _uriBuilder;
        private readonly HttpClientHandler _handler;
        private bool _disposed;

        public JsonHttpClient(IRestUriBuilder uriBuilder)
            : this(uriBuilder, new ExponentialRetryPolicy(), new AutomaticDecompressionHandler())
        {
        }

        public JsonHttpClient(IRestUriBuilder uriBuilder, IRetryPolicy retryPolicy)
           : this(uriBuilder, retryPolicy, new AutomaticDecompressionHandler())
        {
        }

        public JsonHttpClient(IRestUriBuilder uriBuilder, IRetryPolicy retryPolicy, HttpClientHandler handler)
        {
            if (uriBuilder == null)
                throw new ArgumentNullException(nameof(uriBuilder));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            if (retryPolicy == null)
                throw new ArgumentNullException(nameof(retryPolicy));

            _uriBuilder = uriBuilder;
            _handler = handler;
            _retryPolicy = retryPolicy;

            InnerClient = new Lazy<HttpClient>(
                () => new HttpClient(_handler));

            SetDefaultRequestHeaders();
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

                HandleException(response);

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
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false);

                HandleException(response);

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
            var content = new ObjectContent<T>(value, _jsonFormatter);
            var request = new HttpRequestMessage(HttpMethod.Put, requestUri) { Content = content };

            return Send(request);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(Uri requestUri, T value)
        {
            var content = new ObjectContent<T>(value, _jsonFormatter);
            var request = new HttpRequestMessage(HttpMethod.Put, requestUri) { Content = content };

            return await SendAsync(request);
        }

        public HttpResponseMessage Post<T>(Uri requestUri, T value)
        {
            var content = new ObjectContent<T>(value, _jsonFormatter);
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = content };

            return Send(request);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(Uri requestUri, T value)
        {
            var content = new ObjectContent<T>(value, _jsonFormatter);
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

        private void HandleException(HttpResponseMessage responseMessage)
        {
            if (responseMessage == null)
                throw new ArgumentNullException(nameof(responseMessage));

            if (responseMessage.IsSuccessStatusCode)
                return;

            throw new HttpResponseMessageException(responseMessage);
        }

        private void SetAuthentication(HttpRequestMessage request)
        {
            if (Authentication != null)
                request.Headers.Authorization = Authentication;
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
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