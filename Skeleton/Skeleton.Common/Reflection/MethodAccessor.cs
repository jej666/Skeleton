using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Skeleton.Abstraction;

namespace Skeleton.Common.Reflection
{
    [DebuggerDisplay("Name: {Name}")]
    public class MethodAccessor : IMethodAccessor
    {
        private readonly LazyRef<MethodDelegate> _methodDelegate;
        private readonly MethodInfo _methodInfo;
        private readonly string _name;

        private MethodAccessor(MethodInfo methodInfo)
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

        public static IMethodAccessor Create(MethodInfo methodInfo)
        {
            return methodInfo == null
                ? null
                : new MethodAccessor(methodInfo);
        }

        public static IMethodAccessor Create(Type type, string name, Type[] parameterTypes)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (name == null)
                throw new ArgumentNullException("name");

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

            var methodInfo = type.GetMethod(name,
                flags,
                null,
                CallingConventions.Any,
                parameterTypes,
                null) ?? type.GetMethod(name);

            if (methodInfo == null)
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, "Method '{0}' was not found in type {1}.", name, type));

            return new MethodAccessor(methodInfo);
        }

        internal static int GetKey(string name, IEnumerable<Type> parameterTypes)
        {
            unchecked
            {
                var result = name != null ? name.GetHashCode() : 0;
                result = parameterTypes.Aggregate(result,
                    (r, p) => (r*397) ^ (p != null ? p.GetHashCode() : 0));

                return result;
            }
        }
    }
}