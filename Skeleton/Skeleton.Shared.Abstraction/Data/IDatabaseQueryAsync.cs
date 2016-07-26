using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Data
{
    public interface IDatabaseQueryAsync
    {
       Task<IEnumerable<dynamic>> FindAsync(
          string query,
          IDictionary<string, object> parameters);

        Task<IEnumerable<TPoco>> FindAsync<TPoco>(
            string query,
            IDictionary<string, object> parameters)
            where TPoco : class;

        Task<TPoco> FirstOrDefaultAsync<TPoco>(
            string query,
            IDictionary<string, object> parameters)
            where TPoco : class;
    }
}