using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Abstraction.Repository
{
    public interface IAsyncEntityQueryResult<TEntity, in TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        Task<TEntity> FirstOrDefaultAsync();

        Task<TEntity> FirstOrDefaultAsync(TIdentity id);

        Task<IEnumerable<TEntity>> FindAsync();

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> PageAsync(int pageSize, int pageNumber);
    }
}