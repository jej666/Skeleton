using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Skeleton.Common;
using Skeleton.Core.Domain;

namespace Skeleton.Core.Repository
{
    public interface IReadOnlyRepositoryAsync<TEntity, TIdentity> :
        IDisposable,
        IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IQueryBuilder<TEntity, TIdentity> Query { get; }

        Task<TEntity> FirstOrDefaultAsync(TIdentity id);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where);

        Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy);

        Task<IEnumerable<TEntity>> FindAsync(ISqlQuery query);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> PageAllAsync(int pageSize, int pageNumber);

        Task<IEnumerable<TEntity>> PageAsync(
            int pageSize,
            int pageNumber,
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy);
    }
}