using System.Collections.Generic;
using System.Linq;

using FluentAssertions;
using Grobid.NET;
using iTextSharp.text.pdf.parser;
using Xunit;

namespace Grobid.Test.PDF
{
    public class TextBlockFactoryTest
    {
        [Fact]
        public void Test()
        {
            var reader = Sample.Pdf.Create(Sample.Pdf.EssenseLinq);

            var textInfos = new List<TextInfo>();
            var xmlTextExtractionStrategy = new XmlTextExtractionStrategy(textInfos);

            PdfTextExtractor.GetTextFromPage(reader, 1, xmlTextExtractionStrategy);

            var testSubject = new TextBlockFactory();
            var textBlocks = testSubject.Create(textInfos, reader.GetPageSize(1).Height).ToArray();
            textBlocks.Length.Should().Be(118);
        }
    }
}
