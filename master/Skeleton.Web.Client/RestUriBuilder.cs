using System;
using System.Collections.Generic;
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
            : this (host, path, 80)
        {
        }

        private string EnsureEndsWithSlash(string value)
        {
            if (!value.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                value += "/";

            return value;
        }

        public IRestUriBuilder New()
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
            if (!string.IsNullOrEmpty(Path))
                Path = EnsureEndsWithSlash(Path);

            Path += parameter.ToString();
           
            return this;
        }

        public IRestUriBuilder SetQueryParameter(string key, object value)
        {
            Query += string.Format("{0}={1}", key, value);

            return this;
        }

        public IRestUriBuilder SetQueryParameters(IDictionary<string, object> parameters)
        {
            var stringBuilder = new StringBuilder();
            foreach (var param in parameters)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder.Append("&");

                stringBuilder.AppendFormat("{0}={1}", param.Key, param.Value);
            }

            Query = stringBuilder.ToString();

            return this;
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
