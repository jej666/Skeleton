using Skeleton.Abstraction;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Repository
{
    public class ReadRepository<TEntity, TDto> :
            DisposableBase,
            IReadRepository<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        public ReadRepository(
            IEntityMapper<TEntity, TDto> mapper,
            IEntityReader<TEntity> reader)
        {
            Mapper = mapper;
            Query = reader;
        }

        public IEntityReader<TEntity> Query { get; }

        public IEntityMapper<TEntity, TDto> Mapper { get; }

        protected override void DisposeManagedResources()
        {
            Query.Dispose();
        }
    }
}