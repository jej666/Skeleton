using System;
using Skeleton.Common.Reflection;

namespace Skeleton.Core.Domain
{
    public interface IEntity<TEntity, TIdentity> :
        IComparable<TEntity>,
        IEquatable<TEntity>,
        IValidatable<TEntity, TIdentity>,
        IAuditable
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        TIdentity Id { get; }

        IMemberAccessor IdAccessor { get; }
    }
}