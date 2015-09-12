using System.IO;
using System.Reflection;

using iTextSharp.text.pdf;

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

            public static PdfReader Create(string resourcePath)
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Sample.Pdf.EssenseLinq);
                var reader = new PdfReader(stream);
                return reader;
            }
        }
    }
}
