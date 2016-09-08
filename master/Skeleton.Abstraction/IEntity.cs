﻿using System;

namespace Skeleton.Abstraction
{
    public interface IEntity<TEntity> :
            IComparable<TEntity>,
            IEquatable<TEntity>,
            //  IEntityValidatable<TEntity, TIdentity>,
            IEntityDescriptor,
            IEntityAuditable
        where TEntity : class, IEntity<TEntity>
    {
        object Id { get; }

        bool IsTransient();
    }
}