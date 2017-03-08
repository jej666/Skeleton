﻿using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class CrudRepository<TEntity, TDto> :
            ReadRepository<TEntity, TDto>,
            ICrudRepository<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        public CrudRepository(
            IEntityMapper<TEntity, TDto> mapper,
            IEntityReader<TEntity> reader,
            IEntityWriter<TEntity> persistor)
            : base(mapper, reader)
        {
            Store = persistor;
        }

        public IEntityWriter<TEntity> Store { get; }

        protected override void DisposeManagedResources()
        {
            Store.Dispose();
            base.DisposeManagedResources();
        }
    }
}