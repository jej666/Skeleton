using Skeleton.Abstraction.Domain;
using System.Collections.Generic;

namespace Skeleton.Core.Domain
{
    public abstract class EntityValidatorBase<TEntity> : IEntityValidator<TEntity>
        where TEntity : class, IEntity<TEntity>, new()
    {
        private readonly List<IValidationRule> _brokenRules = new List<IValidationRule>();

        protected abstract void Validate(TEntity entity);

        protected void AddBrokenRule(IValidationRule rule)
        {
            _brokenRules.Add(rule);
        }

        public IEnumerable<IValidationRule> BrokenRules(TEntity entity)
        {
            _brokenRules.Clear();
            Validate(entity);

            return _brokenRules;
        }
    }
}