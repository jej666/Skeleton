using System;
using System.Net.Http;

namespace Skeleton.Web.Client
{
    public static class HttpResponseMessageExtensions
    {
        public static void HandleException(this HttpResponseMessage responseMessage)
        {
            if (responseMessage == null)
                throw new ArgumentNullException(nameof(responseMessage));

            if (responseMessage.IsSuccessStatusCode)
                return;

            throw new CustomHttpException(responseMessage.ReasonPhrase, (int)responseMessage.StatusCode);
        }
    }
}