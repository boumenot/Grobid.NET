using System.Xml.XPath;

using FluentAssertions;
using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class ConverterTest
    {
        [Fact]
        public void EssenseShouldSpecificTokenBlocks()
        {
            var testSubject = new Converter();

            var pageBackFactory = new PageBlockFactory();
            var pageBlocks = pageBackFactory.Create(Sample.Pdf.OpenEssenseLinq(), 1);

            var doc = testSubject.ToXml(pageBlocks);
            doc.XPathSelectElements("/DOCUMENT/PAGE/TEXT/TOKEN").Should().HaveCount(859);
        }
    }
}
