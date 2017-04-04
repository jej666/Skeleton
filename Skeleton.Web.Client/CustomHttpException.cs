using System;
using System.Runtime.Serialization;

namespace Skeleton.Web.Client
{
    [Serializable]
    public class CustomHttpException : Exception
    {
        public int StatusCode { get; }

        public CustomHttpException()
        {
        }

        public CustomHttpException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        protected CustomHttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}