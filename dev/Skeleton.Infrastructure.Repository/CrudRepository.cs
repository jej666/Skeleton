﻿using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class CrudRepository<TEntity, TIdentity, TDto> :
            ReadRepository<TEntity, TIdentity, TDto>,
            ICrudRepository<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public CrudRepository(
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            IEntityReader<TEntity, TIdentity> reader,
            IEntityPersitor<TEntity, TIdentity> persistor)
            : base(mapper, reader)
        {
            Store = persistor;
        }

        public IEntityPersitor<TEntity, TIdentity> Store { get; }

        protected override void DisposeManagedResources()
        {
            Store.Dispose();
        }
    }
}