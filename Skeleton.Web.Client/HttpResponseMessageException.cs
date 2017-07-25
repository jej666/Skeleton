using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Skeleton.Web.Client
{
    [Serializable]
    public class HttpResponseMessageException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public HttpResponseMessage Response { get; }

        public HttpResponseMessageException()
        {
        }

        public HttpResponseMessageException(HttpResponseMessage response) 
            : base(response.ReasonPhrase)
        {
            Response = response;
            StatusCode = response.StatusCode;
        }

        protected HttpResponseMessageException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}