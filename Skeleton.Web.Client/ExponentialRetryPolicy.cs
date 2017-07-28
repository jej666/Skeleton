using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public sealed class ExponentialRetryPolicy : IRetryPolicy
    {
        private static readonly TimeSpan DefaultRetryInterval = TimeSpan.FromSeconds(1.0);
        private static readonly TimeSpan DefaultMaxBackoff = TimeSpan.FromSeconds(30.0);
        private static readonly TimeSpan DefaultMinBackoff = TimeSpan.FromSeconds(1.0);
        private static readonly int DefaultRetryCount = 5;

        private static readonly HttpStatusCode[] httpStatusCodesWorthRetrying =
        {
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.BadGateway,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.GatewayTimeout
        };

        private readonly int _maxRetries;
        private readonly TimeSpan _retryInterval;

        public ExponentialRetryPolicy()
            : this(DefaultRetryCount, DefaultRetryInterval)
        {
        }

        public ExponentialRetryPolicy(int maxRetries)
            : this(maxRetries, DefaultRetryInterval)
        {
        }

        public ExponentialRetryPolicy(int maxRetries, TimeSpan retryInterval)
        {
            _maxRetries = maxRetries;
            _retryInterval = retryInterval;
        }

        public int RetryCount
        {
            get;
            private set;
        }

        public TimeSpan DelayInterval
        {
            get;
            private set;
        }

        public T Execute<T>(Func<T> func)
        {
            RetryCount = 0;

            for (;;)
            {
                DelayInterval = CalculateExponentialBackoff();

                try
                {
                    return func();
                }
                // Connection error
                catch (HttpRequestException)
                {
                    ++RetryCount;

                    if (RetryCount == _maxRetries)
                        throw;

                    Task.Delay(DelayInterval).Wait();
                }
                // Enriched exception (statuscode)
                catch (HttpResponseMessageException e)
                {
                    ++RetryCount;

                    if (RetryCount == _maxRetries)
                        throw;

                    if (!httpStatusCodesWorthRetrying.Contains(e.StatusCode))
                        throw;

                    Task.Delay(DelayInterval).Wait();
                }
            }
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> func)
        {
            RetryCount = 0;

            for (;;)
            {
                DelayInterval = CalculateExponentialBackoff();

                try
                {
                    return await func();
                }
                catch (HttpRequestException)
                {
                    ++RetryCount;

                    if (RetryCount == DefaultRetryCount)
                        throw;

                    Task.Delay(DelayInterval).Wait();
                }
                catch (HttpResponseMessageException e)
                {
                    ++RetryCount;

                    if (RetryCount == DefaultRetryCount)
                        throw;

                    if (!httpStatusCodesWorthRetrying.Contains(e.StatusCode))
                        throw;

                    Task.Delay(DelayInterval).Wait();
                }
            }
        }

        private TimeSpan CalculateExponentialBackoff()
        {
            int randomInterval = GetRandomInterval();
            var delta = (int)(Math.Pow(2.0, RetryCount) * randomInterval);
            var interval = (int)Math.Min(checked(DefaultMinBackoff.TotalMilliseconds + delta),
                DefaultMaxBackoff.TotalMilliseconds);

            return TimeSpan.FromMilliseconds(interval);
        }

        private int GetRandomInterval()
        {
            var random = new Random();
            var randomInterval = random.Next(
                (int)(_retryInterval.TotalMilliseconds * 0.8),
                (int)(_retryInterval.TotalMilliseconds * 1.2));

            return randomInterval;
        }
    }
}