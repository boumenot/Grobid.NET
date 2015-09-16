using iTextSharp.text.pdf.parser;

namespace Grobid.PdfToXml
{
    public static class ITextSharpExtensions
    {
        public static float X(this Vector v) { return v[Vector.I1]; }
        public static float Y(this Vector v) { return v[Vector.I2]; }
    }
}
