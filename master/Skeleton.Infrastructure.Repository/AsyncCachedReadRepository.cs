using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class AsyncCachedReadRepository<TEntity, TIdentity, TDto> :
            AsyncReadRepository<TEntity, TIdentity, TDto>,
            IAsyncCachedReadRepository<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public AsyncCachedReadRepository(
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            IAsyncCachedEntityReader<TEntity, TIdentity> reader)
            : base(mapper, reader)
        {
            Query = reader;
        }

        public new IAsyncCachedEntityReader<TEntity, TIdentity> Query { get; }

        protected override void DisposeManagedResources()
        {
            Query.Dispose();
        }
    }
}