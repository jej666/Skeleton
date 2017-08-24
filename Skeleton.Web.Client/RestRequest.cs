using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Skeleton.Web.Client
{
    public sealed class RestRequest : IRestRequest
    {
        private readonly static SupportedFormatters Formatters = new SupportedFormatters();

        public RestRequest()
        {
        }

        public RestRequest(string resource)
        {
            if (string.IsNullOrEmpty(resource))
                throw new ArgumentException(nameof(resource));

            Resource = EnsureTrailingSlash(resource);
        }

        public RestRequest(string resource, HttpMethod method)
            : this(resource)
        {
            Method = method;
        }

        public HttpMethod Method { get; private set; }

        public string Resource { get; private set; }

        public string QueryString { get; private set; }

        public HttpContent Content { get; private set; }

        public IRestRequest WithBody<T>(T value)
        {
            Content = new ObjectContent(typeof(T), value, Formatters.FirstOrDefault());

            return this;
        }

        public IRestRequest AsPut()
        {
            Method = HttpMethod.Put;

            return this;
        }

        public IRestRequest AsDelete()
        {
            Method = HttpMethod.Delete;

            return this;
        }

        public IRestRequest AsPost()
        {
            Method = HttpMethod.Post;

            return this;
        }

        public IRestRequest AsGet()
        {
            Method = HttpMethod.Get;

            return this;
        }

        public IRestRequest AddResource(string resource)
        {
            if (string.IsNullOrEmpty(resource))
                throw new ArgumentException(nameof(resource));

            if (!string.IsNullOrEmpty(Resource))
                Resource = EnsureTrailingSlash(Resource);

            Resource += resource;

            return this;
        }

        public IRestRequest AddQueryParameter(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(nameof(key));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (!string.IsNullOrEmpty(QueryString))
                QueryString += "&";

                QueryString += EncodeUriParameter(new KeyValuePair<string, object>(key, value));

            return this;
        }

        public IRestRequest AddQueryParameters(IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var stringBuilder = new StringBuilder();
            foreach (var parameter in parameters)
            {
                if (parameter.Value == null)
                    continue;

                if (stringBuilder.Length > 0)
                    stringBuilder.Append("&");

                var encodedParameter = EncodeUriParameter(parameter);
                stringBuilder.Append(encodedParameter);
            }

            QueryString = stringBuilder.ToString();

            return this;
        }

        public IRestRequest AddQueryParameters<T>(T value) where T : class, new()
        {
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                var propertyValue = propertyInfo.GetValue(value, null);
                if (propertyValue == null)
                    continue;

                AddQueryParameter(propertyInfo.Name, propertyValue);
            }

            return this;
        }

        public HttpRequestMessage CreateRequestMessage(Uri baseAddress)
        { 
            var builder = new UriBuilder(baseAddress);
            var basePath = string.IsNullOrEmpty(builder.Path) 
                ? string.Empty 
                : EnsureTrailingSlash(builder.Path);
            
            builder.Path = basePath + Resource;
            builder.Query = QueryString;

            var requestMessage = new HttpRequestMessage(Method, builder.Uri);

            if (Content != null)
                requestMessage.Content = Content;

            return requestMessage;
        }

        private static string EncodeUriParameter(KeyValuePair<string, object> parameter)
        {
            var key = Uri.EscapeDataString(parameter.Key);
            var value = Uri.EscapeDataString(parameter.Value.ToString());

            return string.Format(CultureInfo.InvariantCulture, "{0}={1}", key, value);
        }

        private static string EnsureTrailingSlash(string value)
        {
            if (!value.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                value += "/";

            return value;
        }
    }
}