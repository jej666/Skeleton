using System;

namespace Skeleton.Abstraction.Domain
{
    public interface IEntity<TEntity> :
            IEquatable<TEntity>,
            IEntityValidatable<TEntity>,
            IEntityDescriptor,
            IEntityAuditable
        where TEntity : class, IEntity<TEntity>
    {
        object Id { get; }

        bool IsTransient();
    }
}