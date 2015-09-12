using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssertions;

using Grobid.NET;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

using Xunit;

namespace Grobid.Test.PDF
{
    public class TextBlockFactoryTest
    {
        [Fact]
        public void Test()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Sample.Pdf.EssenseLinq);
            var reader = new PdfReader(stream);

            var textInfos = new List<TextInfo>();
            var xmlTextExtractionStrategy = new XmlTextExtractionStrategy(textInfos);

            PdfTextExtractor.GetTextFromPage(reader, 1, xmlTextExtractionStrategy);

            var testSubject = new TextBlockFactory();
            var textBlocks = testSubject.Create(textInfos, reader.GetPageSize(1).Height).ToArray();
            textBlocks.Length.Should().Be(118);
        }
    }
}
