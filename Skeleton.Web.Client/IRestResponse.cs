using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public interface IRestResponse
    {
        T As<T>();
        IEnumerable<T> AsEnumerable<T>();
        Stream AsStream();
        string AsString();

        Task<T> AsAsync<T>();
        Task<IEnumerable<T>> AsEnumerableAsync<T>();
        Task<Stream> AsStreamAsync();
        Task<string> AsStringAsync();

        void EnsureSuccessStatusCode();

        SupportedFormatters Formatters { get; }

        bool IsSuccessStatusCode { get; }

        HttpResponseMessage Message { get; }

        HttpStatusCode StatusCode { get; }
    }
}