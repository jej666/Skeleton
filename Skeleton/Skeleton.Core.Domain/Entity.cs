using System;
using System.Linq.Expressions;
using Skeleton.Common.Extensions;
using Skeleton.Common.Reflection;

namespace Skeleton.Core.Domain
{
    public abstract class Entity<TEntity, TIdentity> :
        IEntity<TEntity, TIdentity>
        where TEntity : Entity<TEntity, TIdentity>
    {
        private readonly IMemberAccessor _idAccessor;

        protected Entity(Expression<Func<TEntity, object>> idExpression)
        {
            idExpression.ThrowIfNull(() => idExpression);

            _idAccessor = PropertyAccessor.Create(idExpression.GetPropertyAccess());
            CreatedDateTime = DateTime.Now;
        }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public TIdentity Id
        {
            get { return (TIdentity) _idAccessor.GetValue(this); }
        }

        public IMemberAccessor IdAccessor
        {
            get { return _idAccessor; }
        }

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
            {
                return false;
            }

            var otherIsTransient = Equals(other.Id, null);
            var currentIsTransient = Equals(Id, null);

            if (otherIsTransient || currentIsTransient)
            {
                return ReferenceEquals(other, this);
            }

            return other.Id.Equals(Id);
        }

        public IValidationResult Validate(IValidator<TEntity, TIdentity> validator)
        {
            validator.ThrowIfNull(() => validator);

            return new ValidationResult(validator.BrokenRules((TEntity) this));
        }

        public static bool operator !=(
            Entity<TEntity, TIdentity> entity1,
            Entity<TEntity, TIdentity> entity2)
        {
            return !(entity1 == entity2);
        }

        public static bool operator ==(
            Entity<TEntity, TIdentity> entity1,
            Entity<TEntity, TIdentity> entity2)
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
            return entity != null
                   && Equals(entity);
        }

        public override string ToString()
        {
            var thisIsTransient = Equals(Id, null);
            return thisIsTransient ? base.ToString() : Id.ToString();
        }

        public override int GetHashCode()
        {
            var thisIsTransient = Equals(Id, null);
            return thisIsTransient ? base.GetHashCode() : Id.GetHashCode();
        }
    }
}