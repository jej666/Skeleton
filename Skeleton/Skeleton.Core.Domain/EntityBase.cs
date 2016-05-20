using System;
using System.Linq.Expressions;
using Skeleton.Common;
using Skeleton.Common.Extensions;
using Skeleton.Common.Reflection;

namespace Skeleton.Core.Domain
{
    public abstract class EntityBase<TEntity, TIdentity> :
        IEntity<TEntity, TIdentity>
        where TEntity : EntityBase<TEntity, TIdentity>
    {
        private readonly IMemberAccessor _idAccessor;

        private readonly LazyRef<ITypeAccessor> _typeAccessor =
            new LazyRef<ITypeAccessor>(() => new TypeAccessor(typeof(TEntity)));

        protected EntityBase(Expression<Func<TEntity, object>> idExpression)
        {
            idExpression.ThrowIfNull(() => idExpression);

            _idAccessor = TypeAccessor.GetProperty(idExpression);
            CreatedDateTime = DateTime.Now;
        }

        public string CreatedBy { get; protected set; }

        public DateTime CreatedDateTime { get; protected set; }

        public TIdentity Id
        {
            get { return (TIdentity) _idAccessor.GetValue(this); }
        }

        public IMemberAccessor IdAccessor
        {
            get { return _idAccessor; }
        }

        public string LastModifiedBy { get; protected set; }

        public DateTime? LastModifiedDateTime { get; protected set; }

        public ITypeAccessor TypeAccessor
        {
            get { return _typeAccessor.Value; }
        }

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
            EntityBase<TEntity, TIdentity> entity1,
            EntityBase<TEntity, TIdentity> entity2)
        {
            return !(entity1 == entity2);
        }

        public static bool operator ==(
            EntityBase<TEntity, TIdentity> entity1,
            EntityBase<TEntity, TIdentity> entity2)
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
            return Id.ToString();
        }

        public override int GetHashCode()
        {
            var thisIsTransient = Equals(Id, null);
            return thisIsTransient ? GetHashCode() : Id.GetHashCode();
        }
    }
}