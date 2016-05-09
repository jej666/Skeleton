namespace Skeleton.Core.Repository
{
    using Core.Domain;

    public interface IExecuteBuilder<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ISqlExecute AsSql();

        IExecuteWhereBuilder<TEntity, TIdentity> Delete(TEntity entity);

        IExecuteBuilder<TEntity, TIdentity> Insert(TEntity entity);

        IExecuteWhereBuilder<TEntity, TIdentity> Update(TEntity entity);
    }
}