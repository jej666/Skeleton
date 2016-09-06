using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class AsyncCrudRepository<TEntity, TIdentity, TDto> :
            AsyncReadRepository<TEntity, TIdentity, TDto>,
            IAsyncCrudRepository<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public AsyncCrudRepository(
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            IAsyncEntityReader<TEntity, TIdentity> reader,
            IAsyncEntityPersistor<TEntity, TIdentity> persistor)
            : base(mapper, reader)
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