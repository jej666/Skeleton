using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Skeleton.Abstraction;

namespace Skeleton.Core.Repository
{
    public interface IAggregateAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        Task<TResult> AverageAsync<TResult>(Expression<Func<TEntity, TResult>> expression);

        Task<int> CountAsync();

        Task<TResult> CountAsync<TResult>(Expression<Func<TEntity, TResult>> expression);

        Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> expression);

        Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> expression);

        Task<TResult> SumAsync<TResult>(Expression<Func<TEntity, TResult>> expression);
    }
}