using System;

namespace Skeleton.Abstraction
{
    public interface ITypeAccessorCache
    {
        ITypeAccessor Get(Type type);

        ITypeAccessor Get<TType>();

        void Remove(Type type);
    }
}