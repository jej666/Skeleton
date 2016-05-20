using Skeleton.Core.Domain;

namespace Skeleton.Core.Repository
{
    public interface IExecuteProcessor<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        bool Delete();

        bool Update();
    }
}