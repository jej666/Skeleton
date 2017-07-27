using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.Core
{
    public static class EnumerableExtensions
    {
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static List<T> AsList<T>(this IEnumerable<T> source)
        {
            return (source == null) || source is List<T>
                ? (List<T>)source
                : source.ToList();
        }

        public static bool FastAny<TSource>(this IEnumerable<TSource> source)
        {
            source.ThrowIfNull(nameof(source));

            var enumerable = source as IList<TSource> ?? source.ToList();
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
            source.ThrowIfNull(nameof(source));
            action.ThrowIfNull(nameof(action));

            var enumerable = source as IList<TSource> ?? source.ToList();

            foreach (var value in enumerable)
                action(value);
        }

        public static bool IsNotNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return (source != null) && source.FastAny();
        }

        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return (source == null) || !source.FastAny();
        }

        public static Task<List<TSource>> ToListAsync<TSource>(this IEnumerable<TSource> source)
        {
            source.ThrowIfNull(nameof(source));

            return Task.Run(() => source.ToList());
        }
    }
}