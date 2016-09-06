using Skeleton.Shared.Abstraction.Reflection;

namespace Skeleton.Shared.Abstraction
{
    public interface IEntityDescriptor
    {
        string IdName { get; }

        IMemberAccessor IdAccessor { get; }

        IMetadata TypeAccessor { get; }
    }
}