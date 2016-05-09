namespace Skeleton.Core.Domain
{
    using Common.Reflection;
    using System;

    public interface IEntity<TEntity, TIdentity> :
        IComparable<TEntity>,
        IEquatable<TEntity>,
        IValidatable<TEntity, TIdentity>,
        IAuditable
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        TIdentity Id { get; }

        IMemberAccessor IdAccessor { get; }
        ITypeAccessor TypeAccessor { get; }
    }
}