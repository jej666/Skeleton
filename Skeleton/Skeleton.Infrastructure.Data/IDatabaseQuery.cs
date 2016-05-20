using System.Collections.Generic;

namespace Skeleton.Infrastructure.Data
{
    public interface IDatabaseQuery
    {
        IEnumerable<TResult> Find<TResult>(
            string query,
            IDictionary<string, object> parameters)
            where TResult : class;

        TResult FirstOrDefault<TResult>(
            string query,
            IDictionary<string, object> parameters)
            where TResult : class;
    }
}