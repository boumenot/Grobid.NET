using System.Text;
using System.Xml;
using System.Xml.Linq;

using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class ConverterTest
    {
        [Fact]
        public void Test()
        {
            var testSubject = new Converter();

            var pageBackFactory = new PageBackFactory();
            var pageBlocks = pageBackFactory.Create(Sample.Pdf.OpenEssenseLinq(), 1);

            var doc = testSubject.ToXml(pageBlocks);
        }
    }

    public class Converter
    {
        public XDocument ToXml(PageBlock[] pageBlocks)
        {
            var sb = new StringBuilder();
            var indexGenerator = new IndexGenerator();

            using (var writer = XmlWriter.Create(sb))
            {
                writer.WriteStartElement("DOCUMENT");

                foreach (var pageBlock in pageBlocks)
                {
                    this.WritePageBlock(indexGenerator, writer, pageBlock);
                }

                writer.WriteEndElement(); // DOCUMENT
            }

            return XDocument.Parse(sb.ToString());
        }

        private void WritePageBlock(IndexGenerator indexGenerator, XmlWriter writer, PageBlock pageBlock)
        {
            writer.WriteStartElement("PAGE");
            writer.WriteAttributeString("width", pageBlock.Width.ToString());
            writer.WriteAttributeString("height", pageBlock.Height.ToString());
            writer.WriteAttributeString("number", pageBlock.Offset.ToString());
            writer.WriteAttributeString("id", indexGenerator.PageIndex);

            writer.WriteEndElement();
        }
    }
}
