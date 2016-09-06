using Skeleton.Core.Repository;
using Skeleton.Core.Service;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Service
{
    public class ReadService<TEntity, TIdentity, TDto> :
        ServiceBase,
        IReadService<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly IEntityReader<TEntity, TIdentity> _reader;
        private readonly IEntityMapper<TEntity, TIdentity, TDto> _mapper;

        public ReadService(
            ILogger logger,
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            IEntityReader<TEntity, TIdentity> reader)
            : base(logger)
        {
            _mapper = mapper;
            _reader = reader;
        }

        public IEntityReader<TEntity, TIdentity> Query
        {
            get { return _reader; }
        }

        public IEntityMapper<TEntity, TIdentity, TDto> Mapper
        {
            get { return _mapper; }
        }

        protected override void DisposeManagedResources()
        {
            _reader.Dispose();
        }
    }
}