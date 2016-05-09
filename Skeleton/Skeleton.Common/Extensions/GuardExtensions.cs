namespace Skeleton.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    [DebuggerStepThrough]
    public static class GuardExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters"),
        SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static void ThrowIfNull<T>(this T value, Expression<Func<T>> reference)
        {
            if (value.Equals(default(T)))
            {
                throw new ArgumentNullException(reference.GetMemberExpression().Member.Name);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters"),
        SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static void ThrowIfNullOrEmpty<T>(this IEnumerable<T> value, Expression<Func<IEnumerable<T>>> reference)
        {
            if (value.IsNullOrEmpty())
            {
                throw new ArgumentNullException(reference.GetMemberExpression().Member.Name);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static void ThrowIfNullOrEmpty(this string value, Expression<Func<string>> reference)
        {
            value.ThrowIfNull(reference);

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Argument cannot be null", reference.GetMemberExpression().Member.Name);
            }
        }
    }
}