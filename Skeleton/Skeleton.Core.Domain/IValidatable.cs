namespace Skeleton.Core.Domain
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Validatable")]
    public interface IValidatable<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IValidationResult Validate(IValidator<TEntity, TIdentity> validator);
    }
}