namespace Skeleton.Core.Domain
{
    using System.Collections.Generic;

    public interface IValidator<TEntity, TIdentity> where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IEnumerable<string> BrokenRules(TEntity entity);
    }
}