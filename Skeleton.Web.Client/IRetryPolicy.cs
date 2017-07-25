using System;
using System.Threading.Tasks;

namespace Skeleton.Web.Client
{
    public interface IRetryPolicy
    {
        int RetryCount { get; }

        TimeSpan DelayInterval { get; }

        T Execute<T>(Func<T> func);

        Task<T> ExecuteAsync<T>(Func<Task<T>> func);
    }
}