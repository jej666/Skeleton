namespace Skeleton.Common.Reflection
{
    using System;
   
    public interface ITypeAccessorCache
    {
        ITypeAccessor Get(Type type);

        ITypeAccessor Get<TType>();

        void Remove(Type type);
    }
}