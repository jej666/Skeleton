using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Skeleton.Abstraction.Repository
{
    public interface IAsyncEntityAggregateResult<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        Task<int> CountAsync();

        Task<IEnumerable<dynamic>> AverageAsync(
            Expression<Func<TEntity, object>> expression);

        Task<IEnumerable<dynamic>> CountAsync(
            Expression<Func<TEntity, object>> expression);

        Task<IEnumerable<dynamic>> MaxAsync(
            Expression<Func<TEntity, object>> expression);

        Task<IEnumerable<dynamic>> MinAsync(
            Expression<Func<TEntity, object>> expression);

        Task<IEnumerable<dynamic>> SumAsync(
            Expression<Func<TEntity, object>> expression);
    }
}