using System;

namespace Skeleton.Shared.Abstraction
{
    public interface IEntity<TEntity, out TIdentity> :
        IComparable<TEntity>,
        IEquatable<TEntity>,
        //  IEntityValidatable<TEntity, TIdentity>,
        IEntityDescriptor,
        IEntityAuditable
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        TIdentity Id { get; }

        bool IsTransient();
    }
}