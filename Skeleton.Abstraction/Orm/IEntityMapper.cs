using Skeleton.Abstraction.Domain;
using System.Collections.Generic;

namespace Skeleton.Abstraction.Orm
{
    public interface IEntityMapper<TEntity, TDto> :
            IHideObjectMethods
        where TEntity : class, IEntity<TEntity>, new()
        where TDto : class, new()
    {
        TDto Map(TEntity entity);

        TEntity Map(TDto dto);

        IEnumerable<TDto> Map(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> Map(IEnumerable<TDto> dtos);
    }
}