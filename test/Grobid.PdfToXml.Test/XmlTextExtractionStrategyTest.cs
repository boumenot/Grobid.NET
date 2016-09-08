using System.Collections.Generic;

using FluentAssertions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class XmlTextExtractionStrategyTest
    {
        [Fact]
        public void Test()
        {
            var reader = new PdfReader(Sample.Pdf.OpenEssenseLinq());

            var tokenBlocks = new List<TokenBlock>();
            var pageSize = reader.GetPageSize(1);
            var tokenBlockFactory = new TokenBlockFactory(pageSize.Width, pageSize.Height);

            var xmlTextExtractionStrategy = new XmlTextExtractionStrategy(tokenBlocks, tokenBlockFactory);

            PdfTextExtractor.GetTextFromPage(reader, 1, xmlTextExtractionStrategy);
            tokenBlocks.Should().NotBeEmpty();
        }
    }
}
