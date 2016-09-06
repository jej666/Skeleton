using System.Collections.Generic;

namespace Skeleton.Abstraction
{
    public interface IEntityValidator<in TEntity, TIdentity> : IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IEnumerable<string> BrokenRules(TEntity entity);
    }
}