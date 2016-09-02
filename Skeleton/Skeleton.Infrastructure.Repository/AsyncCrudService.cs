using Skeleton.Core;
using Skeleton.Core.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public class AsyncCrudService<TEntity, TIdentity, TDto> :
            AsyncReadService<TEntity, TIdentity, TDto>,
            IAsyncCrudService<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public AsyncCrudService(
            ILogger logger,
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            IAsyncEntityReader<TEntity, TIdentity> reader,
            IAsyncEntityPersistor<TEntity, TIdentity> persistor)
            : base(logger, mapper, reader)
        {
            Store = persistor;
        }

        public IAsyncEntityPersistor<TEntity, TIdentity> Store { get; }

        protected override void DisposeManagedResources()
        {
            Store.Dispose();
        }
    }
}