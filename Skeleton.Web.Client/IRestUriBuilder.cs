using System;
using System.Collections.Generic;

namespace Skeleton.Web.Client
{
    public interface IRestUriBuilder
    {
        Uri Uri { get; }

        IRestUriBuilder AppendAction(RestAction action);

        IRestUriBuilder AppendAction(object parameter);

        IRestUriBuilder StartNew();

        IRestUriBuilder SetQueryParameter(string key, object value);

        IRestUriBuilder SetQueryParameters(IDictionary<string, object> parameters);
    }
}