using System;
using System.Linq;
using System.Net;
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
            get
            {
                int randomInterval = GetRandomInterval();
                var delta = (int)(Math.Pow(2.0, RetryCount) * randomInterval);
                var interval = (int)Math.Min(checked(DefaultMinBackoff.TotalMilliseconds + delta),
                    DefaultMaxBackoff.TotalMilliseconds);

                return TimeSpan.FromMilliseconds(interval);
            }
        }

        public async Task<IRestResponse> ExecuteAsync(Func<Task<IRestResponse>> func)
        {
            IRestResponse response = null;
            for (RetryCount = 0; RetryCount < _maxRetries; RetryCount++)
            {
                if (RetryCount != 0)
                    Task.Delay(DelayInterval).Wait();
                
                try
                {
                    response = await func().ConfigureAwait(false);
                }
                catch (Exception)
                {
                    throw;
                }

                if (!httpStatusCodesWorthRetrying.Contains(response.StatusCode))
                    break;
            }
            return response;
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