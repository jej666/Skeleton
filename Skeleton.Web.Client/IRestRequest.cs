using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Skeleton.Web.Client
{
    public interface IRestRequest
    {
        HttpRequestMessage CreateRequestMessage(Uri baseAddress);

        IRestRequest AddQueryParameter(string key, object value);
        IRestRequest AddQueryParameters(IDictionary<string, object> parameters);
        IRestRequest AddQueryParameters<T>(T value) where T : class, new();
        IRestRequest AddResource(string resource);
        IRestRequest AsPut();
        IRestRequest AsDelete();
        IRestRequest AsPost();
        IRestRequest AsGet();
        IRestRequest WithBody<T>(T value);

        HttpMethod Method { get; }
        string QueryString { get; }
        string Resource { get; }
        HttpContent Content { get; }
    }
}