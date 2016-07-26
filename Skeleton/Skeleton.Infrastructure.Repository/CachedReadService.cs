using Skeleton.Core.Repository;
using Skeleton.Core.Service;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Service
{
    public class CachedReadService<TEntity, TIdentity, TDto> :
        ReadService<TEntity, TIdentity, TDto>,
        ICachedReadService<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly ICachedEntityReader<TEntity, TIdentity> _reader;

        public CachedReadService(
            ILogger logger,
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            ICachedEntityReader<TEntity, TIdentity> reader)
            : base(logger, mapper, reader)
        {
            _reader = reader;
        }

        public new ICachedEntityReader<TEntity, TIdentity> Query
        {
            get { return _reader; }
        }

        protected override void DisposeManagedResources()
        {
            _reader.Dispose();
        }
    }
}