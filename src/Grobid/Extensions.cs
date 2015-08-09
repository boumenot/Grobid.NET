using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
