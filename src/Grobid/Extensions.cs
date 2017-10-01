using System;
using System.Collections.Generic;

namespace Grobid.NET
{
    public static class Extensions
    {
        public static U GetValueOrDefault<T, U>(this IDictionary<T, U> dict, T key, U defaultValue)
        {
            U value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            foreach (var item in collection)
            {
                yield return item;

                if (predicate(item))
                {
                    yield break;
                }
            }
        }

        public static void EnsureNotNull(this object o, string name)
        {
            if (o == null)
            {
                throw new ArgumentNullException($"The parameter {name} is null!");
            }
        }

        public static void EnsureNotEmpty(this string s, string name)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentOutOfRangeException($"The parameter {name} is empty!");
            }
        }
    }
}
