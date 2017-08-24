using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public sealed class NoRetryPolicy : IRetryPolicy
    {
        public int RetryCount { get; } = 0;

        public TimeSpan DelayInterval { get; } = TimeSpan.MinValue;

        public async Task<IRestResponse> ExecuteAsync(Func<Task<IRestResponse>> func)
        {
            try
            {
                return await func().ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}