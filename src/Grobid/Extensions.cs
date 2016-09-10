using System.Collections.Generic;

namespace Grobid.NET
{
    public static class Extensions
    {
        public static U GetValueOrDefault<T,U>(this IDictionary<T, U> dict, T key, U defaultValue)
        {
            U value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }
    }
}
