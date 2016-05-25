using System;
using Skeleton.Common;
using Skeleton.Common.Extensions;
using Skeleton.Common.Reflection;
using Skeleton.Core.Domain;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Repository.SqlBuilder;

namespace Skeleton.Infrastructure.Repository
{
    public abstract class EntityRepository<TEntity, TIdentity> :
        DisposableBase,
        IEntityRepository<TEntity,TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly LazyRef<ITypeAccessor> _typeAccessor;

        protected EntityRepository(ITypeAccessorCache typeAccessorCache)
        {
            typeAccessorCache.ThrowIfNull(() => typeAccessorCache);

            _typeAccessor = new LazyRef<ITypeAccessor>(typeAccessorCache.Get<TEntity>);
            InitializeBuilder();
        }

        public ITypeAccessor TypeAccessor
        {
            get { return _typeAccessor.Value; }
        }

        protected SqlBuilderManager Builder { get; private set; }

        protected void InitializeBuilder()
        {
            Builder = new SqlBuilderManager(typeof(TEntity));
        }

        protected T HandleBuilderInitialization<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            finally
            {
                InitializeBuilder();
            }
        }
    }
}