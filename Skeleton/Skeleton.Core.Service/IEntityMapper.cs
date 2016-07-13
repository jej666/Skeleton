using Skeleton.Shared.Abstraction;

namespace Skeleton.Core.Service
{
    public interface IEntityMapper<TEntity, in TIdentity, TDto> :
        IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        TDto Map(TEntity entity);

        TEntity Map(TDto dto);

        TEntity Map(TIdentity id, TDto dto);
    }
}