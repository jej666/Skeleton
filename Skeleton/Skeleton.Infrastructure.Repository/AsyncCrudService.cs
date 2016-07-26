using System;
using Skeleton.Core.Repository;
using Skeleton.Core.Service;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Service
{
    public class AsyncCrudService<TEntity, TIdentity, TDto> :
        AsyncReadService<TEntity, TIdentity, TDto>,
        IAsyncCrudService<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly IAsyncEntityPersistor<TEntity, TIdentity> _persistor;

        public AsyncCrudService(
            ILogger logger,
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            IAsyncEntityReader<TEntity,TIdentity> reader,
            IAsyncEntityPersistor<TEntity, TIdentity> persistor)
            : base(logger, mapper, reader)
        {
            _persistor = persistor;
        }

        public IAsyncEntityPersistor<TEntity, TIdentity> Store
        {
            get { return _persistor; }
        }

        protected override void DisposeManagedResources()
        {
            _persistor.Dispose();
        }
    }
}