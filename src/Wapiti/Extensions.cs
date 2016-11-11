using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Wapiti
{
    public static class Extensions
    {
        public static string[] ReadAllLines(this TextReader reader)
        {
            return Extensions.ReadAllLinesImpl(reader).ToArray();
        }

        private static IEnumerable<string> ReadAllLinesImpl(TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}
