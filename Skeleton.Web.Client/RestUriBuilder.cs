using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Skeleton.Web.Client
{
    public class RestUriBuilder : UriBuilder, IRestUriBuilder
    {
        private readonly string _initialPath;

        public RestUriBuilder(string host, int port, string path)
        {
            Host = host;
            Port = port;
            Path = EnsureEndsWithSlash(path);
            _initialPath = Path;
        }

        public RestUriBuilder(UriBuilder builder)
            : this(builder.Host, builder.Port, builder.Path)
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

                case RestAction.Query:
                    Path += ActionConstants.Query;
                    break;

                case RestAction.FirstOrDefault:
                    Path += ActionConstants.FirstOrDefault;
                    break;

                case RestAction.Create:
                    Path += ActionConstants.Create;
                    break;

                case RestAction.BatchCreate:
                    Path += ActionConstants.BatchCreate;
                    break;

                case RestAction.Update:
                    Path += ActionConstants.Update;
                    break;

                case RestAction.BatchUpdate:
                    Path += ActionConstants.BatchUpdate;
                    break;

                case RestAction.Delete:
                    Path += ActionConstants.Delete;
                    break;

                case RestAction.BatchDelete:
                    Path += ActionConstants.BatchDelete;
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
                throw new ArgumentException("Key cannot be null or empty", nameof(key));

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
                if (parameter.Value == null)
                    continue;

                if (stringBuilder.Length > 0)
                    stringBuilder.Append("&");

                var encodedParameter = EncodeUriParameter(parameter);

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
            internal const string GetAll = "getall";
            internal const string Query = "query";
            internal const string FirstOrDefault = "firstordefault";
            internal const string Create = "create";
            internal const string BatchCreate = "batchcreate";
            internal const string Update = "update";
            internal const string BatchUpdate = "batchupdate";
            internal const string Delete = "delete";
            internal const string BatchDelete = "batchdelete";
        }
    }
}