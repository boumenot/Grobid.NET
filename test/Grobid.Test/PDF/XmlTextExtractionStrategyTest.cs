using System.Collections.Generic;

using FluentAssertions;
using Grobid.NET;
using iTextSharp.text.pdf.parser;
using Xunit;

namespace Grobid.Test.PDF
{
    public class XmlTextExtractionStrategyTest
    {
        [Fact]
        public void Test()
        {
            var reader = Sample.Pdf.Create(Sample.Pdf.EssenseLinq);

            var textInfos = new List<TextInfo>();
            var xmlTextExtractionStrategy = new XmlTextExtractionStrategy(textInfos);

            PdfTextExtractor.GetTextFromPage(reader, 1, xmlTextExtractionStrategy);
            textInfos.Should().NotBeEmpty();
        }
    }
}
