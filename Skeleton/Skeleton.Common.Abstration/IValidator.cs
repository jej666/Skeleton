using System.Collections.Generic;

namespace Skeleton.Abstraction
{
    public interface IValidator<in TEntity, TIdentity> 
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IEnumerable<string> BrokenRules(TEntity entity);
    }
}