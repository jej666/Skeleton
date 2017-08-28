using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public sealed class RestResponse : IRestResponse
    {
        public RestResponse(HttpResponseMessage response, SupportedFormatters formatters)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (formatters == null)
                throw new ArgumentNullException(nameof(formatters));

            Message = response;
            Formatters = formatters;
        }

        public SupportedFormatters Formatters { get; private set; }

        public HttpResponseMessage Message { get; private set; }

        public bool IsSuccessStatusCode => Message.IsSuccessStatusCode;

        public HttpStatusCode StatusCode => Message.StatusCode;

        public void EnsureSuccessStatusCode()
        {
            Message.EnsureSuccessStatusCode();
        }

        public string AsString()
        {
            return AsStringAsync().Result;
        }

        public Task<string> AsStringAsync()
        {
            return Message.Content.ReadAsStringAsync();
        }

        public Stream AsStream()
        {
            var stream = AsStreamAsync().Result;
            stream.Position = 0;

            return stream;
        }

        public Task<Stream> AsStreamAsync()
        {
            return Message.Content.ReadAsStreamAsync();
        }

        public IEnumerable<T> AsEnumerable<T>()
        {
            return As<IEnumerable<T>>();
        }

        public Task<IEnumerable<T>> AsEnumerableAsync<T>()
        {
            return AsAsync<IEnumerable<T>>();
        }

        public T As<T>()
        {
            return AsAsync<T>().Result;
        }

        public Task<T> AsAsync<T>()
        {
            return Message.Content.ReadAsAsync<T>(Formatters);
        }
    }
}