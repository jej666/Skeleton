using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Abstraction.Data
{
    public interface IDatabaseQueryAsync
    {
        Task<IEnumerable<dynamic>> FindAsync(ISqlCommand command);

        Task<IEnumerable<TPoco>> FindAsync<TPoco>(ISqlCommand command)
            where TPoco : class;

        Task<TPoco> FirstOrDefaultAsync<TPoco>(ISqlCommand command)
            where TPoco : class;
    }
}