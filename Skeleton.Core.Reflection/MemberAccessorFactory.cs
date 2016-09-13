using System;
using System.Globalization;
using System.Reflection;
using Skeleton.Abstraction;

namespace Skeleton.Core.Reflection
{
    public static class MemberAccessorFactory
    {
        public static IMemberAccessor Create(FieldInfo fieldInfo)
        {
            return fieldInfo == null ? null : new FieldAccessor(fieldInfo);
        }

        public static IMemberAccessor Create(PropertyInfo propertyInfo)
        {
            return propertyInfo == null
                ? null
                : new PropertyAccessor(propertyInfo);
        }

        public static IMethodAccessor Create(Type type, string name, Type[] parameterTypes)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

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
    }
}