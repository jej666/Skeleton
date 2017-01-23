using System;

namespace Skeleton.Abstraction.Reflection
{
    public interface IInstanceAccessor
    {
        Func<object[], object> InstanceCreator { get; }
    }
}