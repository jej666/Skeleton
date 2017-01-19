using System.Collections.Generic;

namespace Skeleton.Abstraction.Domain
{
    public interface IEntityValidator<in TEntity> : IHideObjectMethods
        where TEntity : class, IEntity<TEntity>
    {
        IEnumerable<string> BrokenRules(TEntity entity);
    }
}