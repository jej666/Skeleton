using Skeleton.Abstraction.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Abstraction.Repository
{
    public interface IAsyncEntityQueryResult<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        Task<TEntity> FirstOrDefaultAsync();

        Task<TEntity> FirstOrDefaultAsync(object id);

        Task<IEnumerable<TEntity>> FindAsync();

        Task<IEnumerable<TEntity>> PageAsync(int pageSize, int pageNumber);
    }
}