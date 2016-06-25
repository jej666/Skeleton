using Skeleton.Abstraction;
using Skeleton.Core.Repository;
using Skeleton.Core.Service;
using System;

namespace Skeleton.Infrastructure.Service
{
    public class ReadService<TEntity, TIdentity> :
        EntityService<TEntity, TIdentity>,
        IReadService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IReadRepository<TEntity, TIdentity> _readRepository;

        public ReadService(
            ILogger logger,
            IReadRepository<TEntity, TIdentity> readRepository)
            : base(logger)
        {
            readRepository.ThrowIfNull(() => readRepository);

            _readRepository = readRepository;
        }

        public IReadRepository<TEntity, TIdentity> Repository
        {
            get { return _readRepository; }
        }

        protected override void DisposeManagedResources()
        {
            _readRepository.Dispose();
        }
    }
}