using FluentAssertions;
using iTextSharp.text.pdf.parser;
using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class TokenBlockFactoryTest
    {
        [Fact]
        public void FactoryShouldNormalizeUnicodeStrings()
        {
            var stub = new TextRenderInfoStub
            {
                AscentLine = new LineSegment(new Vector(0, 0, 0), new Vector(1, 1, 1)),
                Baseline = new LineSegment(new Vector(0, 0, 0), new Vector(1, 1, 1)),
                DescentLine = new LineSegment(new Vector(0, 0, 0), new Vector(1, 1, 1)),
                PostscriptFontName = "CHUFSU+NimbusRomNo9L-Medi",
                Text = "abcd\u0065\u0301fgh",
            };

            var testSubject = new TokenBlockFactory(100, 100);
            var tokenBlock = testSubject.Create(stub);

            stub.Text.ToCharArray().Should().HaveCount(9);
            tokenBlock.Text.ToCharArray().Should().HaveCount(8);
        }
    }
}
