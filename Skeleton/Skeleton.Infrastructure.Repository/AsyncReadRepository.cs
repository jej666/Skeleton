using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public class AsyncReadRepository<TEntity, TIdentity, TDto> :
            RepositoryBase,
            IAsyncReadRepository<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public AsyncReadRepository(
            ILogger logger,
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            IAsyncEntityReader<TEntity, TIdentity> reader)
            : base(logger)
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