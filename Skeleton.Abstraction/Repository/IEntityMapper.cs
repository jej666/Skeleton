using Skeleton.Abstraction.Domain;
using System.Collections.Generic;

namespace Skeleton.Abstraction.Repository
{
    public interface IEntityMapper<TEntity, TDto> :
            IHideObjectMethods
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        TDto Map(TEntity entity);

        TEntity Map(TDto dto);

        IEnumerable<TDto> Map(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> Map(IEnumerable<TDto> dtos);
    }
}