using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grobid.Test
{
    public static class Extensions
    {
        public static Stream ToStream(this string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            return new MemoryStream(bytes);
        }

        public static Stream ToStream(this IEnumerable<string> xs)
        {
            return Extensions.ToStream(String.Join(Environment.NewLine, xs.ToArray()));
        }
    }
}
