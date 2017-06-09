using Skeleton.Abstraction;
using Skeleton.Abstraction.Dependency;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Data
{
    internal sealed class ExponentialRetryPolicy
    {
        private readonly IDatabaseConfiguration _configuration;
        private readonly ILogger _logger;

        internal static readonly TimeSpan DefaultMaxBackoff = TimeSpan.FromSeconds(30.0);
        internal static readonly TimeSpan DefaultMinBackoff = TimeSpan.FromSeconds(1.0);
        internal static readonly bool FirstFastRetry = true;
        private int _currentRetry;

        internal ExponentialRetryPolicy(IDatabaseConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        internal T Execute<T>(Func<T> func)
        {
            _currentRetry = 0;

            for(;;)
            {
                try
                {
                    return func();
                }
                catch (SqlException e)
                {
                    var exponentialInterval = CalculateExponentialBackoff();

                    _currentRetry++;

                    _logger.Error("Database error => ", e);

                    if (_currentRetry == _configuration.RetryCount)
                        throw;

                    if ((e.Number != 1205) && (e.Number != -2))
                        throw;

                    Task.Delay(exponentialInterval).Wait();
                }
            }
        }

        internal async Task<T> ExecuteAsync<T>(Func<Task<T>> func)
        {
            _currentRetry = 0;

            for(;;)
            { 
                try
                {
                    return await func();
                }
                catch (SqlException e)
                {
                    var exponentialInterval = CalculateExponentialBackoff();

                    _currentRetry++;

                    _logger.Error("Database error => ", e);

                    if (_currentRetry == _configuration.RetryCount)
                        throw;

                    if ((e.Number != 1205) && (e.Number != -2))
                        throw;

                    Task.Delay(exponentialInterval).Wait();
                }
            }
        }

        private TimeSpan CalculateExponentialBackoff()
        {
            var defaultInterval = TimeSpan.FromSeconds(_configuration.RetryInterval);

            if (FirstFastRetry && _currentRetry == 0)
                return defaultInterval;

            int randomInterval = GetRandomInterval(defaultInterval);
            var delta = (int)(Math.Pow(2.0, _currentRetry) * randomInterval);
            var interval = (int)Math.Min(checked(DefaultMinBackoff.TotalMilliseconds + delta), DefaultMaxBackoff.TotalMilliseconds);

            return TimeSpan.FromMilliseconds(interval);
        }

        private static int GetRandomInterval(TimeSpan defaultInterval)
        {
            var random = new Random();
            var randomInterval = random.Next((int)(defaultInterval.TotalMilliseconds * 0.8),
                (int)(defaultInterval.TotalMilliseconds * 1.2));

            return randomInterval;
        }
    }
}