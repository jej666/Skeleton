using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Reflection;
using Skeleton.Common;
using Skeleton.Core.Reflection;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Skeleton.Core.Domain
{
    [DebuggerDisplay(" Id = {ToString()}")]
    public abstract class Entity<TEntity> :
            IEntity<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        private const int HashMultiplier = 31;
        private int? _cachedHashcode;

        protected Entity(Expression<Func<TEntity, object>> idExpression)
        {
            idExpression.ThrowIfNull(() => idExpression);

            TypeAccessor = new MetadataProvider().GetMetadata(typeof(TEntity));
            IdAccessor = TypeAccessor.GetProperty(idExpression);
            CreatedDateTime = DateTime.Now;
        }

        public object Id => IdAccessor.Getter(this);

        public string IdName => IdAccessor.Name;

        public IMemberAccessor IdAccessor { get; }

        public IMetadata TypeAccessor { get; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        public virtual int CompareTo(TEntity other)
        {
            if (other.GetType() != GetType())
                throw new InvalidOperationException("Invalid type");

            return Equals(other) ? 0 : 1;
        }

        public virtual bool Equals(TEntity other)
        {
            if (other == null)
                return false;

            var otherIsTransient = Equals(other.Id, null);

            if (otherIsTransient || IsTransient())
                return ReferenceEquals(other, this);

            return other.Id.Equals(Id);
        }

        public virtual bool IsTransient()
        {
            return (Id == null) || Id.Equals(default(object));
        }

        public IValidationResult Validate(IEntityValidator<TEntity> validator)
        {
            validator.ThrowIfNull(() => validator);

            return new EntityValidationResult(validator.BrokenRules((TEntity)this));
        }

        public static bool operator !=(
            Entity<TEntity> entity1,
            Entity<TEntity> entity2)
        {
            return !(entity1 == entity2);
        }

        public static bool operator ==(
            Entity<TEntity> entity1,
            Entity<TEntity> entity2)
        {
            return Equals(entity1, entity2);
        }

        public int CompareTo(object other)
        {
            return CompareTo(other as TEntity);
        }

        public override bool Equals(object obj)
        {
            var entity = obj as TEntity;

            return (entity != null)
                   && Equals(entity);
        }

        public override string ToString()
        {
            return IsTransient()
                ? base.ToString()
                : Id.ToString();
        }

        public override int GetHashCode()
        {
            if (_cachedHashcode != null)
                return _cachedHashcode.Value;

            if (IsTransient())
                _cachedHashcode = base.GetHashCode();
            else
                unchecked
                {
                    var hashCode = GetType().GetHashCode();
                    _cachedHashcode = (hashCode * HashMultiplier) ^ Id.GetHashCode();
                }
            return _cachedHashcode.Value;
        }
    }
}