using Skeleton.Abstraction.Reflection;

namespace Skeleton.Abstraction
{
    public interface IEntityDescriptor
    {
        string IdName { get; }

        IMemberAccessor IdAccessor { get; }

        IMetadata TypeAccessor { get; }
    }
}