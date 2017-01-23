using System;

namespace Skeleton.Abstraction.Reflection
{
    public interface IMemberAccessor : IMemberInfo
    {
        Func<object, object> Getter { get; }

        Action<object, object> Setter { get; }
    }
}