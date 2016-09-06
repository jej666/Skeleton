using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Data
{
    public interface IDatabaseQueryAsync
    {
        Task<IEnumerable<TResult>> FindAsync<TResult>(
            string query,
            IDictionary<string, object> parameters)
            where TResult : class;

        Task<TResult> FirstOrDefaultAsync<TResult>(
            string query,
            IDictionary<string, object> parameters)
            where TResult : class;
    }
}