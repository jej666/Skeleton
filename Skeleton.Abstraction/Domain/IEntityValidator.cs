using System.Collections.Generic;

namespace Skeleton.Abstraction.Domain
{
    public interface IEntityValidator<in TEntity> : IHideObjectMethods
        where TEntity : class, IEntity<TEntity>, new()
    {
        IEnumerable<IValidationRule> BrokenRules(TEntity entity);
    }
}