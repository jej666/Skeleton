using System.Collections.Generic;
using Skeleton.Core.Domain;

namespace Skeleton.Core.Repository
{
    public interface IRepository<TEntity, TIdentity> :
        IReadOnlyRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ISqlExecute SqlExecute { get; }

        bool Add(TEntity entity);

        bool Add(IEnumerable<TEntity> entities);

        bool Delete(TEntity entity);

        bool Delete(IEnumerable<TEntity> entities);

        bool Save(TEntity entity);

        bool Save(IEnumerable<TEntity> entities);

        bool Update(TEntity entity);

        bool Update(IEnumerable<TEntity> entities);

        //IExecuteProcessor<TEntity, TIdentity> WherePrimaryKey(
        //    IEnumerable<object> values);
        //    Expression<Func<TEntity, object>> expression,

        //IExecuteProcessor<TEntity, TIdentity> WhereNotIn(
        //    IEnumerable<object> values);
        //    Expression<Func<TEntity, object>> expression,

        //IExecuteProcessor<TEntity, TIdentity> WhereIsIn(
        //    Expression<Func<TEntity, bool>> expression);

        //IExecuteProcessor<TEntity, TIdentity> Where(
        //    Expression<Func<TEntity, bool>> whereExpression);
    }
}