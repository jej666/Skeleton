﻿using Skeleton.Abstraction.Reflection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.Common.Reflection
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
    }
}
