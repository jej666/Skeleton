using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Skeleton.Shared.Abstraction;
using System.Collections.Generic;

namespace Skeleton.Core.Repository
{
    public interface IAsyncEntityAggregateResult<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
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