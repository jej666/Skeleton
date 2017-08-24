using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Skeleton.Core
{
    [DebuggerStepThrough]
    public static class GuardExtensions
    {
        public static void ThrowIfNull<T>(this T value, [CallerMemberName] string parameterName = null)
        {
            if (value.Equals(default(T)))
                throw new ArgumentNullException(parameterName ?? "object");
        }

        public static void ThrowIfNullOrEmpty<T>(this IEnumerable<T> value, [CallerMemberName] string parameterName = null)
        {
            if (value.IsNullOrEmpty())
                throw new ArgumentNullException(parameterName ?? "collection");
        }

        public static void ThrowIfNullOrEmpty(this string value, [CallerMemberName] string parameterName = null)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Argument cannot be null", parameterName ?? "string");
        }
    }
}