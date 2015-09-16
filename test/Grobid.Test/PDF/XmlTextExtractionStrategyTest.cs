using System.Collections.Generic;

using FluentAssertions;
using Grobid.NET;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Xunit;

namespace Grobid.Test.PDF
{
    public class XmlTextExtractionStrategyTest
    {
        [Fact]
        public void Test()
        {
            var reader = new PdfReader(Sample.Pdf.OpenEssenseLinq());

            var tokenBlocks = new List<TokenBlock>();
            var pageSize = reader.GetPageSize(1);
            var xmlTextExtractionStrategy = new XmlTextExtractionStrategy(tokenBlocks, pageSize.Width, pageSize.Height);

            PdfTextExtractor.GetTextFromPage(reader, 1, xmlTextExtractionStrategy);
            tokenBlocks.Should().NotBeEmpty();
        }
    }
}
