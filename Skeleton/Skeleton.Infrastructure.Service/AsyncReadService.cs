using System;
using Skeleton.Core.Repository;
using Skeleton.Core.Service;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Service
{
    public class AsyncReadService<TEntity, TIdentity, TDto> :
        ServiceBase,
        IAsyncReadService<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        private readonly IAsyncEntityReader<TEntity, TIdentity> _reader;
        private readonly IEntityMapper<TEntity, TIdentity, TDto> _mapper;

        public AsyncReadService(
            ILogger logger,
            IEntityMapper<TEntity, TIdentity, TDto> mapper,
            IAsyncEntityReader<TEntity, TIdentity> reader)
            : base(logger)
        {
            _reader = reader;
            _mapper = mapper;
        }

        public IAsyncEntityReader<TEntity, TIdentity> Query
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