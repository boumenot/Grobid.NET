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

            var textInfos = new List<TokenBlock>();
            var xmlTextExtractionStrategy = new XmlTextExtractionStrategy(textInfos);

            PdfTextExtractor.GetTextFromPage(reader, 1, xmlTextExtractionStrategy);
            textInfos.Should().NotBeEmpty();
        }
    }
}
