using Skeleton.Core;
using Skeleton.Core.Repository;

namespace Skeleton.Infrastructure.Repository
{
    public class ReadService<TEntity, TIdentity, TDto> :
            ServiceBase,
            IReadService<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        public ReadService(
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