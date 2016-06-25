using Skeleton.Abstraction;
using Skeleton.Abstraction.Reflection;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Repository.SqlBuilder;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Skeleton.Infrastructure.Repository
{
    [DebuggerDisplay("EntityName = {EntityType.Name}")]
    public abstract class EntityRepository<TEntity, TIdentity> :
        IEntityRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IMetadata _metadata;
        private string _cacheIdName;
        private bool _disposed;

        protected EntityRepository(IMetadataProvider metadataProvider)
        {
            metadataProvider.ThrowIfNull(() => metadataProvider);

            _metadata = metadataProvider.GetMetadata<TEntity>();
            InitializeSqlBuilder();
        }

        protected SqlBuilderManager Builder { get; private set; }

        protected string EntityIdName
        { 
            get 
            {
                if (_cacheIdName.IsNullOrEmpty())
                {
                    var instance = _metadata.CreateInstance<TEntity>();
                    if (instance == null)
                        return string.Empty;

                    _cacheIdName = instance.IdAccessor.Name;
                }
                return _cacheIdName; 
            } 
        }

        public Type EntityType 
        { 
            get { return typeof(TEntity); } 
        }

        protected void InitializeSqlBuilder()
        {
            Builder = new SqlBuilderManager(EntityType);
        }

        protected T HandleSqlBuilderInitialization<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            finally
            {
                InitializeSqlBuilder();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~EntityRepository()
        {
            Dispose(false);
        }

        protected virtual void DisposeManagedResources()
        {
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                DisposeManagedResources();

            _disposed = true;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }
    }
}