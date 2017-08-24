using System;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public interface IRetryPolicy
    {
        int RetryCount { get; }

        TimeSpan DelayInterval { get; }

        Task<IRestResponse> ExecuteAsync(Func<Task<IRestResponse>> func);
    }
}