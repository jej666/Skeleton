using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public class RestClient : IRestClient
    {
        private readonly static int ClientVersion = Assembly.GetAssembly(typeof(RestClient)).GetName().Version.Major;
        private readonly static SupportedFormatters Formatters = new SupportedFormatters();
        private static Lazy<HttpClient> InnerClient;

        private readonly IRetryPolicy _retryPolicy;
        private bool _disposed;

        public RestClient(Uri baseAddress)
            : this(baseAddress, new ExponentialRetryPolicy(), new AutomaticDecompressionHandler())
        {
        }

        public RestClient(Uri baseAddress, IRetryPolicy retryPolicy)
           : this(baseAddress, retryPolicy, new AutomaticDecompressionHandler())
        {
        }

        public RestClient(Uri baseAddress, IRetryPolicy retryPolicy, HttpMessageHandler handler)
        {
            if (baseAddress == null)
                throw new ArgumentNullException(nameof(baseAddress));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            if (retryPolicy == null)
                throw new ArgumentNullException(nameof(retryPolicy));

            _retryPolicy = retryPolicy;

            InnerClient = new Lazy<HttpClient>(
                () => new HttpClient(handler));

            ConfigureHttpClient(baseAddress, handler);
        }

        public AuthenticationHeaderValue Authentication { get; set; }

        public Uri BaseAddress => Client.BaseAddress;

        public IRetryPolicy RetryPolicy => _retryPolicy;

        private static HttpClient Client => InnerClient.Value;

        protected virtual async Task<IRestResponse> SendAsync(IRestRequest request)
        {
            CheckDisposed();

            var requestMessage = request.CreateRequestMessage(BaseAddress);
            SetAuthentication(requestMessage);

            return await RetryPolicy.ExecuteAsync(async () =>
            {
                var requestClone = RetryPolicy.RetryCount > 0
                ? Clone(requestMessage)
                : requestMessage;

                var response = await Client
                    .SendAsync(requestClone)
                    .ConfigureAwait(false);

                return new RestResponse(response, Formatters);
            });
        }

        public IRestResponse Get(IRestRequest request)
        {
            return GetAsync(request).Result;
        }

        public async Task<IRestResponse> GetAsync(IRestRequest request)
        {
            EnsureRequestNotNull(request);

            return await SendAsync(request.AsGet()).ConfigureAwait(false);
        }

        public T Get<T>(IRestRequest request)
        {
            return GetAsync<T>(request).Result;
        }

        public async Task<T> GetAsync<T>(IRestRequest request)
        {
            EnsureRequestNotNull(request);

            return await GetAsync(request)
                .ContinueWith(response => response.Result.As<T>());
        }

        public IRestResponse Get(Action<IRestRequest> requestBuilder)
        {
            return GetAsync(requestBuilder).Result;
        }

        public async Task<IRestResponse> GetAsync(Action<IRestRequest> requestBuilder)
        {
            EnsureRequestBuilderNotNull(requestBuilder);

            var request = new RestRequest().AsGet();
            requestBuilder(request);

            return await SendAsync(request).ConfigureAwait(false);
        }

        public T Get<T>(Action<IRestRequest> requestBuilder)
        {
            return GetAsync<T>(requestBuilder).Result;
        }

        public async Task<T> GetAsync<T>(Action<IRestRequest> requestBuilder)
        {
            return await GetAsync(requestBuilder)
                .ContinueWith(response => response.Result.As<T>());
        }

        public IRestResponse Delete(IRestRequest request)
        {
            return DeleteAsync(request).Result;
        }

        public async Task<IRestResponse> DeleteAsync(IRestRequest request)
        {
            EnsureRequestNotNull(request);

            return await SendAsync(request.AsDelete()).ConfigureAwait(false);
        }

        public IRestResponse Delete(Action<IRestRequest> requestBuilder)
        {
            return DeleteAsync(requestBuilder).Result;
        }

        public async Task<IRestResponse> DeleteAsync(Action<IRestRequest> requestBuilder)
        {
            EnsureRequestBuilderNotNull(requestBuilder);

            var request = new RestRequest().AsDelete();
            requestBuilder(request);

            return await SendAsync(request).ConfigureAwait(false);
        }

        public IRestResponse Put(IRestRequest request)
        {
            return PutAsync(request).Result;
        }

        public async Task<IRestResponse> PutAsync(IRestRequest request)
        {
            EnsureRequestNotNull(request);

            return await SendAsync(request.AsPut())
                .ConfigureAwait(false);
        }

        public IRestResponse Put(Action<IRestRequest> requestBuilder)
        {
            return PutAsync(requestBuilder).Result;
        }

        public async Task<IRestResponse> PutAsync(Action<IRestRequest> requestBuilder)
        {
            EnsureRequestBuilderNotNull(requestBuilder);

            var request = new RestRequest().AsPut();
            requestBuilder(request);

            return await SendAsync(request).ConfigureAwait(false);
        }

        public IRestResponse Post(IRestRequest request)
        {
            return PostAsync(request).Result;
        }

        public async Task<IRestResponse> PostAsync(IRestRequest request)
        {
            EnsureRequestNotNull(request);

            return await SendAsync(request.AsPost()).ConfigureAwait(false);
        }

        public T Post<T>(IRestRequest request)
        {
            return PostAsync<T>(request).Result;
        }

        public async Task<T> PostAsync<T>(IRestRequest request)
        {
            EnsureRequestNotNull(request);

            return await SendAsync(request.AsPost())
                .ContinueWith(response => response.Result.As<T>());
        }

        public IRestResponse Post(Action<IRestRequest> requestBuilder)
        {
            return PostAsync(requestBuilder).Result;
        }

        public async Task<IRestResponse> PostAsync(Action<IRestRequest> requestBuilder)
        {
            EnsureRequestBuilderNotNull(requestBuilder);

            var request = new RestRequest().AsPost();
            requestBuilder(request);

            return await SendAsync(request).ConfigureAwait(false);
        }

        public T Post<T>(Action<IRestRequest> requestBuilder)
        {
            return PostAsync<T>(requestBuilder).Result;
        }

        public async Task<T> PostAsync<T>(Action<IRestRequest> requestBuilder)
        {
            EnsureRequestBuilderNotNull(requestBuilder);

            var request = new RestRequest().AsPost();
            requestBuilder(request);

            return await SendAsync(request)
                .ContinueWith(response => response.Result.As<T>());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static void ConfigureHttpClient(Uri baseAddress, HttpMessageHandler handler)
        {
            Client.BaseAddress = baseAddress;

            Client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            Client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
            Client.DefaultRequestHeaders.UserAgent.Add(GetUserAgent());

            Client.DefaultRequestHeaders.Accept.Clear();
            foreach (var mediaTypeHeader in Formatters.SelectMany(x => x.SupportedMediaTypes))
                Client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(mediaTypeHeader.MediaType));

            var clientHandler = handler as HttpClientHandler;
            if (clientHandler == null)
                return;

            clientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        }

        private static ProductInfoHeaderValue GetUserAgent()
        {
            return new ProductInfoHeaderValue(
                new ProductHeaderValue(
                    string.Format(CultureInfo.InvariantCulture, Constants.ProductHeader, ClientVersion)));
        }

        private static void EnsureRequestNotNull(IRestRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
        }

        private static void EnsureRequestBuilderNotNull(Action<IRestRequest> requestBuilder)
        {
            if (requestBuilder == null)
                throw new ArgumentNullException(nameof(requestBuilder));
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

        private static HttpRequestMessage Clone(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Content = request.Content,
                Version = request.Version
            };

            foreach (var prop in request.Properties)
                clone.Properties.Add(prop);

            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            return clone;
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