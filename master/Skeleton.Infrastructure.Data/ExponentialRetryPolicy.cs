using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Data
{
    internal sealed class ExponentialRetryPolicy
    {
        private readonly IDatabaseConfiguration _configuration;
        private readonly ILogger _logger;

        public static readonly TimeSpan DefaultMaxBackoff = TimeSpan.FromSeconds(30.0);
        public static readonly TimeSpan DefaultMinBackoff = TimeSpan.FromSeconds(1.0);

        internal ExponentialRetryPolicy(IDatabaseConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        internal T Execute<T>(Func<T> func)
        {
            var retryCount = _configuration.RetryCount;
            var exponentialInterval = CalculateExponentialRetryInterval();

            while (true)
            {
                try
                {
                    return func();
                }
                catch (SqlException e)
                {
                    --retryCount;

                    _logger.Error("Database error => ", e);

                    if (retryCount <= 0)
                        throw;

                    if ((e.Number != 1205) && (e.Number != -2))
                        throw;

                    Task.Delay(exponentialInterval).Wait();
                }
            }
        }

        internal async Task<T> Execute<T>(Func<Task<T>> func)
        {
            var retryCount = _configuration.RetryCount;
            var exponentialInterval = CalculateExponentialRetryInterval();

            while (true)
            {
                try
                {
                    return await func();
                }
                catch (SqlException e)
                {
                    --retryCount;

                    _logger.Error("Database error => ", e);

                    if (retryCount <= 0)
                        throw;

                    if ((e.Number != 1205) && (e.Number != -2))
                        throw;

                    Task.Delay(exponentialInterval).Wait();
                }
            }
        }

        private TimeSpan CalculateExponentialRetryInterval()
        {
            var random = new Random();
            var retryInterval = TimeSpan.FromSeconds(_configuration.RetryInterval);
            var delta = (int)((Math.Pow(2.0, _configuration.RetryCount) - 1.0) *
                random.Next((int)(retryInterval.TotalMilliseconds * 0.8),
                (int)(retryInterval.TotalMilliseconds * 1.2)));
            var interval = (int)Math.Min(checked(DefaultMinBackoff.TotalMilliseconds + delta),
                DefaultMaxBackoff.TotalMilliseconds);

            return TimeSpan.FromMilliseconds(interval);
        }
    }
}