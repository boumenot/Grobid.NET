using System.IO;
using System.Reflection;

namespace Grobid.PdfToXml.Test
{
    public static class Sample
    {
        public static class Pdf
        {
            public static string EssenseLinq = "Grobid.PdfToXml.Test.essence-linq.pdf";

            public static Stream OpenEssenseLinq()
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Sample.Pdf.EssenseLinq);
                return stream;
            }
        }
    }
}
