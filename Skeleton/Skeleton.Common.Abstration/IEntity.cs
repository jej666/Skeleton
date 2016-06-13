using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Skeleton.Abstraction
{
    public interface IEntity<TEntity, TIdentity> :
        IComparable<TEntity>,
        IEquatable<TEntity>,
        IValidatable<TEntity, TIdentity>,
        IAuditable
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        TIdentity Id { get; }

        PropertyInfo IdAccessor { get; }
    }
}