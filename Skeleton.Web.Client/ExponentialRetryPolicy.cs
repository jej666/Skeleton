using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    internal sealed class ExponentialRetryPolicy
    {
        internal static readonly TimeSpan DefaultRetryInterval = TimeSpan.FromSeconds(1.0);
        internal static readonly TimeSpan DefaultMaxBackoff = TimeSpan.FromSeconds(30.0);
        internal static readonly TimeSpan DefaultMinBackoff = TimeSpan.FromSeconds(1.0);
        internal static readonly int DefaultRetryCount = 10;
        internal static readonly bool FirstFastRetry = true;
        internal static readonly int[] httpStatusCodesWorthRetrying = { 408, 500, 502, 503, 504 };

        internal T Execute<T>(Func<T> func)
        {
            var retryCount = 0;

            for (;;)
            {
                var exponentialInterval = CalculateExponentialBackoff(retryCount);

                try
                {
                    return func();
                }
                // Connection error
                catch (HttpRequestException)
                {
                    ++retryCount;

                    if (retryCount == DefaultRetryCount)
                        throw;

                    Task.Delay(exponentialInterval).Wait();
                }
                // Enriched exception (statuscode)
                catch (CustomHttpException e)
                {
                    ++retryCount;

                    if (retryCount == DefaultRetryCount)
                        throw;

                    if (!httpStatusCodesWorthRetrying.Contains(e.StatusCode))
                        throw;

                    Task.Delay(exponentialInterval).Wait();
                }
            }
        }

        internal async Task<T> ExecuteAsync<T>(Func<Task<T>> func)
        {
            var retryCount = 0;

            for (;;)
            {
                var exponentialInterval = CalculateExponentialBackoff(retryCount);

                try
                {
                    return await func();
                }
                catch (HttpRequestException)
                {
                    ++retryCount;

                    if (retryCount == DefaultRetryCount)
                        throw;

                    Task.Delay(exponentialInterval).Wait();
                }
                catch (CustomHttpException e)
                {
                    ++retryCount;

                    if (retryCount == DefaultRetryCount)
                        throw;

                    if (!httpStatusCodesWorthRetrying.Contains(e.StatusCode))
                        throw;

                    Task.Delay(exponentialInterval).Wait();
                }
            }
        }

        private TimeSpan CalculateExponentialBackoff(int retryCount)
        {
            if (FirstFastRetry && retryCount == 0)
                return DefaultRetryInterval;

            var random = new Random();
            var randomInterval = random.Next((int)(DefaultRetryInterval.TotalMilliseconds * 0.8),
                (int)(DefaultRetryInterval.TotalMilliseconds * 1.2));
            var delta = (int)(Math.Pow(2.0, retryCount) * randomInterval);
            var interval = (int)Math.Min(checked(DefaultMinBackoff.TotalMilliseconds + delta),
                DefaultMaxBackoff.TotalMilliseconds);

            return TimeSpan.FromMilliseconds(interval);
        }
    }
}