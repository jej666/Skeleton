﻿using System;

namespace Skeleton.Common.Reflection
{
    public interface ITypeAccessorCache
    {
        ITypeAccessor Get(Type type);

        ITypeAccessor Get<TType>();

        void Remove(Type type);
    }
}