using Skeleton.Common;
using Skeleton.Common.Extensions;
using Skeleton.Core.Domain;
using Skeleton.Core.Repository;
using Skeleton.Core.Service;

namespace Skeleton.Infrastructure.Service
{
    public class ReadOnlyService<TEntity, TIdentity> :
        EntityService<TEntity, TIdentity>,
        IReadOnlyService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IReadOnlyRepository<TEntity, TIdentity> _readOnlyRepository;

        public ReadOnlyService(
            ILogger logger,
            IReadOnlyRepository<TEntity, TIdentity> readOnlyRepository)
            : base(logger)
        {
            readOnlyRepository.ThrowIfNull(() => readOnlyRepository);

            _readOnlyRepository = readOnlyRepository;
        }

        public IReadOnlyRepository<TEntity, TIdentity> Repository
        {
            get { return _readOnlyRepository; }
        }

        protected override void DisposeManagedResources()
        {
            _readOnlyRepository.Dispose();
        }
    }
}