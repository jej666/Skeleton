using System;
using System.Diagnostics;
using System.Reflection;
using Skeleton.Abstraction.Reflection;

namespace Skeleton.Common.Reflection
{
    [DebuggerDisplay("Name: {Name}")]
    public class MethodAccessor : IMethodAccessor
    {
        private readonly LazyRef<MethodDelegate> _methodDelegate;
        private readonly MethodInfo _methodInfo;
        private readonly string _name;

        public MethodAccessor(MethodInfo methodInfo)
        {
            methodInfo.ThrowIfNull(() => methodInfo);

            _methodInfo = methodInfo;
            _name = methodInfo.Name;

            _methodDelegate = new LazyRef<MethodDelegate>(() =>
                DelegateFactory.CreateMethod(methodInfo));
        }

        public MethodInfo MethodInfo
        {
            get { return _methodInfo; }
        }

        public string Name
        {
            get { return _name; }
        }

        public object Invoke(object instance, params object[] arguments)
        {
            instance.ThrowIfNull(() => instance);

            return _methodDelegate.Value == null
                ? null
                : _methodDelegate.Value(instance, arguments);
        }
    }
}