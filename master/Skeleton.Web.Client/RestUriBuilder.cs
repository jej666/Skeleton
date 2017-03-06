using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Skeleton.Web.Client
{
    public class RestUriBuilder : UriBuilder, IRestUriBuilder
    {
        private readonly string _initialPath;

        public RestUriBuilder(string host, string path, int port)
        {
            Host = host;
            Port = port;
            Path = EnsureEndsWithSlash(path);
            _initialPath = Path;
        }

        public RestUriBuilder(string host, string path)
            : this(host, path, 80)
        {
        }

        public IRestUriBuilder StartNew()
        {
            Path = _initialPath;
            Fragment = string.Empty;
            Query = string.Empty;

            return this;
        }

        public IRestUriBuilder AppendAction(RestAction action)
        {
            switch (action)
            {
                case RestAction.GetAll:
                    Path += ActionConstants.GetAll;
                    break;
                case RestAction.Get:
                    Path += ActionConstants.Get;
                    break;
                case RestAction.Add:
                    Path += ActionConstants.Add;
                    break;
                case RestAction.AddMany:
                    Path += ActionConstants.AddMany;
                    break;
                case RestAction.Page:
                    Path += ActionConstants.Page;
                    break;
                case RestAction.Update:
                    Path += ActionConstants.Update;
                    break;
                case RestAction.UpdateMany:
                    Path += ActionConstants.UpdateMany;
                    break;
                case RestAction.Delete:
                    Path += ActionConstants.Delete;
                    break;
                case RestAction.DeleteMany:
                    Path += ActionConstants.DeleteMany;
                    break;
                default:
                    break;
            }

            return this;
        }

        public IRestUriBuilder AppendAction(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            if (!string.IsNullOrEmpty(Path))
                Path = EnsureEndsWithSlash(Path);

            Path += parameter.ToString();

            return this;
        }

        public IRestUriBuilder SetQueryParameter(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(nameof(key));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Query += EncodeUriParameter(new KeyValuePair<string, object>(key, value));
            
            return this;
        }

        public IRestUriBuilder SetQueryParameters(IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var stringBuilder = new StringBuilder();
            foreach (var parameter in parameters)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder.Append("&");

                var encodedParameter= EncodeUriParameter(parameter);

                stringBuilder.Append(encodedParameter);
            }

            Query = stringBuilder.ToString();

            return this;
        }

        private static string EncodeUriParameter(KeyValuePair<string, object> parameter)
        {
            var key = Uri.EscapeDataString(parameter.Key);
            var value = Uri.EscapeDataString(parameter.Value.ToString());

            return string.Format(CultureInfo.InvariantCulture, "{0}={1}", key, value);
        }

        private static string EnsureEndsWithSlash(string value)
        {
            if (!value.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                value += "/";

            return value;
        }

        private static class ActionConstants
        {
            internal const string GetAll = "GetAll";
            internal const string Get = "Get";
            internal const string Add = "Add";
            internal const string AddMany = "AddMany";
            internal const string Page = "Page";
            internal const string Update = "Update";
            internal const string UpdateMany = "UpdateMany";
            internal const string Delete = "Delete";
            internal const string DeleteMany = "DeleteMany";
        }
    }
}
