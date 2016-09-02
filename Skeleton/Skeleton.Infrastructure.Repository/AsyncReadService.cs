using Skeleton.Core;
using Skeleton.Core.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public class AsyncReadService<TEntity, TIdentity, TDto> :
            ServiceBase,
            IAsyncReadService<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public AsyncReadService(
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