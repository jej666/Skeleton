using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Core.Repository
{
    public interface IAsyncEntityAggregateResult<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        Task<int> CountAsync();

        Task<TResult> AverageAsync<TResult>(Expression<Func<TEntity, TResult>> expression);

        Task<TResult> CountAsync<TResult>(Expression<Func<TEntity, TResult>> expression);

        Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> expression);

        Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> expression);

        Task<TResult> SumAsync<TResult>(Expression<Func<TEntity, TResult>> expression);
    }
}