using Skeleton.Shared.Abstraction;
using System.Collections.Generic;

namespace Skeleton.Infrastructure.Data
{
    public interface IDatabaseQuery
    {
        IEnumerable<dynamic> Find(
            string query,
            IDictionary<string, object> parameters);

        IEnumerable<TPoco> Find<TPoco>(
            string query,
            IDictionary<string, object> parameters)
            where TPoco : class;

        TPoco FirstOrDefault<TPoco>(
            string query,
            IDictionary<string, object> parameters)
            where TPoco : class;
    }
}