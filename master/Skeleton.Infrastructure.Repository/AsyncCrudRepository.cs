using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class AsyncCrudRepository<TEntity, TDto> :
            AsyncReadRepository<TEntity, TDto>,
            IAsyncCrudRepository<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        public AsyncCrudRepository(
            IEntityMapper<TEntity, TDto> mapper,
            IAsyncEntityReader<TEntity> reader,
            IAsyncEntityWriter<TEntity> persistor)
            : base(mapper, reader)
        {
            Store = persistor;
        }

        public IAsyncEntityWriter<TEntity> Store { get; }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            Store.Dispose();
        }
    }
}