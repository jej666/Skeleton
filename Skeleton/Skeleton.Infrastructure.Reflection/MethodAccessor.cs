using System;
using System.Diagnostics;
using System.Reflection;
using Skeleton.Core.Reflection;
using Skeleton.Shared.CommonTypes;

namespace Skeleton.Infrastructure.Reflection
{
    [DebuggerDisplay("Name: {Name}")]
    public class MethodAccessor : IMethodAccessor
    {
        private readonly LazyRef<MethodDelegate> _methodDelegate;

        public MethodAccessor(MethodInfo methodInfo)
        {
            methodInfo.ThrowIfNull(() => methodInfo);

            MethodInfo = methodInfo;
            Name = methodInfo.Name;

            _methodDelegate = new LazyRef<MethodDelegate>(() =>
                    DelegateFactory.CreateMethod(methodInfo));
        }

        public MethodInfo MethodInfo { get; }

        public string Name { get; }

        public object Invoke(object instance, params object[] arguments)
        {
            instance.ThrowIfNull(() => instance);

            return _methodDelegate.Value == null
                ? null
                : _methodDelegate.Value(instance, arguments);
        }
    }
}