using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Repository
{
    public class AsyncReadRepository<TEntity, TIdentity, TDto> :
            DisposableBase,
            IAsyncReadRepository<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public AsyncReadRepository(
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            IAsyncEntityReader<TEntity, TIdentity> reader)
        {
            Query = reader;
            Mapper = mapper;
        }

        public IAsyncEntityReader<TEntity, TIdentity> Query { get; }

        public IEntityMapper<TEntity, TIdentity, TDto> Mapper { get; }

        protected override void DisposeManagedResources()
        {
            Query.Dispose();
        }
    }
}