namespace Skeleton.Core.Repository
{
    using Core.Domain;

    public interface ISqlExecuteBuilder<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ISqlExecute AsSql();

        ISqlExecuteWhereBuilder<TEntity, TIdentity> Delete(TEntity entity);

        ISqlExecuteBuilder<TEntity, TIdentity> Insert(TEntity entity);

        ISqlExecuteWhereBuilder<TEntity, TIdentity> Update(TEntity entity);
    }
}