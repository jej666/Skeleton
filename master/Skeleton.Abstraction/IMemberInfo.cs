﻿using System;
using System.Reflection;

namespace Skeleton.Abstraction
{
    public interface IMemberInfo : IHideObjectMethods
    {
        bool HasGetter { get; }
        bool HasSetter { get; }
        MemberInfo MemberInfo { get; }
        Type MemberType { get; }
        string Name { get; }
    }
}