using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> And<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other)
        {
            if (source == null)
                return other;

            if (other == null)
                return source;

            return source.Concat(other);
        }

        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static List<T> AsList<T>(this IEnumerable<T> source)
        {
            return source == null || source is List<T>
                ? (List<T>) source
                : source.ToList();
        }

        public static IEnumerable<TSource> Distinct<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> expression)
        {
            return source == null
                ? Enumerable.Empty<TSource>()
                : source.GroupBy(expression).Select(i => i.First());
        }

        public static bool FastAny<TSource>(this IEnumerable<TSource> source)
        {
            var enumerable = source as IList<TSource> ?? source.ToList();
            enumerable.ThrowIfNull(() => enumerable);

            var collection = source as ICollection<TSource>;

            if (collection != null)
                return collection.Count > 0;

            using (var enumerator = enumerable.GetEnumerator())
            {
                if (enumerator.MoveNext())
                    return true;
            }
            return false;
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            var enumerable = source as IList<TSource> ?? source.ToList();
            enumerable.ThrowIfNull(() => enumerable);
            action.ThrowIfNull(() => action);

            foreach (var value in enumerable)
                action(value);
        }

        public static void ForEachWhileTrue<T>(this IEnumerable<T> that, Func<T, bool> action)
        {
            foreach (var item in that)
            {
                if (!action(item)) break;
            }
        }

        public static IEnumerable<TSource> IgnoreNulls<TSource>(this IEnumerable<TSource> source)
        {
            if (ReferenceEquals(source, null))
                yield break;

            foreach (var item in source.Where(item => !ReferenceEquals(item, null)))
                yield return item;
        }

        public static bool IsNotNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source != null && source.FastAny();
        }

        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source == null || !source.FastAny();
        }

        public static IEnumerable<TSource> Only<TSource>(
            this IEnumerable<TSource> source,
            IEnumerable<TSource> allowed)
        {
            foreach (var item in source)
            {
                var enumerable = allowed as IList<TSource> ?? allowed.ToList();
                if (enumerable.Contains(item))
                    yield return item;
            }
        }

        public static IEnumerable<TSource> RemoveWhere<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null)
                yield break;

            foreach (var t in source.Where(t => !predicate(t)))
                yield return t;
        }

        public static IEnumerable<TSource> WhereNotNull<TSource>(
            this IEnumerable<TSource> source) where TSource : class
        {
            return source == null
                ? Enumerable.Empty<TSource>()
                : source.Where(x => x != null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static IEnumerable<TSource> WhereNotNull<TSource>(this IEnumerable<TSource?> source)
            where TSource : struct
        {
            return source == null
                ? Enumerable.Empty<TSource>()
                : source.Where(x => x.HasValue).Select(x => x.Value);
        }
    }
}