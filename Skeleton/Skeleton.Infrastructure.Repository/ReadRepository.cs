using Skeleton.Core;
using Skeleton.Core.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public class ReadRepository<TEntity, TIdentity, TDto> :
            RepositoryBase,
            IReadRepository<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public ReadRepository(
            ILogger logger,
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            IEntityReader<TEntity, TIdentity> reader)
            : base(logger)
        {
            Mapper = mapper;
            Query = reader;
        }

        public IEntityReader<TEntity, TIdentity> Query { get; }

        public IEntityMapper<TEntity, TIdentity, TDto> Mapper { get; }

        protected override void DisposeManagedResources()
        {
            Query.Dispose();
        }
    }
}