namespace Skeleton.Common.Reflection
{
    using System;
    using System.Reflection;

    public interface IMemberInfo
    {
        bool HasGetter { get; }
        bool HasSetter { get; }
        MemberInfo MemberInfo { get; }
        Type MemberType { get; }
        string Name { get; }
    }
}