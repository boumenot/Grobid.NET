using System.IO;
using System.Reflection;

namespace Grobid.Test
{
    public static class Sample
    {
        public static class Pdf
        {
            public static string EssenseLinq = "Grobid.Test.essence-linq.pdf";

            public static Stream OpenEssenseLinq()
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Sample.Pdf.EssenseLinq);
                return stream;
            }
        }
    }
}
