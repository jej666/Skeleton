﻿namespace Skeleton.Abstraction
{
    public interface IEntityValidatable<out TEntity> : IHideObjectMethods
        where TEntity : class, IEntity<TEntity>
    {
        IEntityValidationResult Validate(IEntityValidator<TEntity> validator);
    }
}