using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Repository
{
    public class AsyncReadRepository<TEntity, TDto> :
            DisposableBase,
            IAsyncReadRepository<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        public AsyncReadRepository(
            IEntityMapper<TEntity, TDto> mapper,
            IAsyncEntityReader<TEntity> reader)
        {
            Query = reader;
            Mapper = mapper;
        }

        public IAsyncEntityReader<TEntity> Query { get; }

        public IEntityMapper<TEntity, TDto> Mapper { get; }

        protected override void DisposeManagedResources()
        {
            Query.Dispose();
            base.DisposeManagedResources();
        }
    }
}