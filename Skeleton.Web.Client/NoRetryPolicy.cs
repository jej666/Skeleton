using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public sealed class NoRetryPolicy : IRetryPolicy
    {
        public int RetryCount { get; } = 0;

        public TimeSpan DelayInterval { get; } = TimeSpan.MinValue;

        public T Execute<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            // Connection error
            catch (HttpRequestException)
            {
                throw;
            }
            // Enriched exception (statuscode)
            catch (HttpResponseMessageException)
            {
                throw;
            }
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> func)
        {
            try
            {
                return await func().ConfigureAwait(false);
            }
            // Connection error
            catch (HttpRequestException)
            {
                throw;
            }
            // Enriched exception (statuscode)
            catch (HttpResponseMessageException)
            {
                throw;
            }
        }
    }
}