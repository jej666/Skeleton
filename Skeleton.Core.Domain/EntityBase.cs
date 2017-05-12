using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Reflection;
using Skeleton.Common;
using Skeleton.Core.Reflection;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Skeleton.Core.Domain
{
    [DebuggerDisplay(" Id = {ToString()}")]
    public abstract class EntityBase<TEntity> : IEntity<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        private const int HashMultiplier = 31;

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected EntityBase(Expression<Func<TEntity, object>> idExpression)
        {
            idExpression.ThrowIfNull(nameof( idExpression));

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

        public IEntityValidationResult Validate(IEntityValidator<TEntity> validator)
        {
            validator.ThrowIfNull(nameof( validator));

            return new EntityValidationResult(validator.BrokenRules(this as TEntity));
        }

        public static bool operator !=(EntityBase<TEntity> entity1, EntityBase<TEntity> entity2)
        {
            return !(entity1 == entity2);
        }

        public static bool operator ==(EntityBase<TEntity> entity1, EntityBase<TEntity> entity2)
        {
            return Equals(entity1, entity2);
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
            if (IsTransient())
                return base.GetHashCode();

            unchecked
            {
                var hashCode = GetType().GetHashCode();
                return (hashCode * HashMultiplier) ^ Id.GetHashCode();
            }
        }
    }
}