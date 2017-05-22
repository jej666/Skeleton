using Skeleton.Abstraction.Reflection;

namespace Skeleton.Abstraction.Domain
{
    public interface IEntityDescriptor
    {
        string IdName { get; }

        IMemberAccessor IdAccessor { get; }

        IMetadata Metadata { get; }
    }
}