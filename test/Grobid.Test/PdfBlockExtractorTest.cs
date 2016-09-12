using System.Linq;

using FluentAssertions;
using Xunit;

using Grobid.NET;
using Grobid.PdfToXml;

namespace Grobid.Test
{
    public class PdfBlockExtractorTest
    {
        private static readonly FontName FontA = FontName.Parse("CHUFSU+NimbusRomNo9L-Medi");

        [Fact]
        public void Test()
        {
            var tokenBlock = new TokenBlock { Text = "Text", FontName = PdfBlockExtractorTest.FontA };
            var textBlock = new TextBlock(new[] { tokenBlock });
            var block = new Block { TextBlocks = new[] { textBlock } };

            var testSubject = new PdfBlockExtractor<string>();
            var result = testSubject.Extract(Enumerable.Repeat(block, 1), x => "MyType")
                    .Single();

            result.Should().Be("MyType");
        }
    }
}
