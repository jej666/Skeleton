using System;

namespace Skeleton.Abstraction
{
    public interface IEntity<TEntity, TIdentity> :
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