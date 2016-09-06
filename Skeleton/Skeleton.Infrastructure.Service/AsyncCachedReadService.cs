using Skeleton.Core.Repository;
using Skeleton.Core.Service;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Service
{
    public class AsyncCachedReadService<TEntity, TIdentity, TDto> :
        AsyncReadService<TEntity, TIdentity, TDto>,
        IAsyncCachedReadService<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly IAsyncCachedEntityReader<TEntity, TIdentity> _reader;

        public AsyncCachedReadService(
            ILogger logger,
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            IAsyncCachedEntityReader<TEntity, TIdentity> reader)
            : base(logger, mapper, reader)
        {
            _reader = reader;
        }

        public new IAsyncCachedEntityReader<TEntity, TIdentity> Query
        {
            get { return _reader; }
        }

        protected override void DisposeManagedResources()
        {
            _reader.Dispose();
        }
    }
}