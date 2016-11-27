using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Grobid.PdfToXml
{
    public sealed class Converter
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

            foreach (var block in pageBlock.Blocks)
            {
                this.WriteBlock(indexGenerator, writer, block);
            }

            writer.WriteEndElement();
        }

        private void WriteBlock(IndexGenerator indexGenerator, XmlWriter writer, Block block)
        {
            writer.WriteStartElement("BLOCK");
            writer.WriteAttributeString("id", indexGenerator.BlockIndex);

            foreach (var textBlock in block.TextBlocks)
            {
                this.WriteTextBlock(indexGenerator, writer, textBlock);
            }

            writer.WriteEndElement();
        }

        private void WriteTextBlock(IndexGenerator indexGenerator, XmlWriter writer, TextBlock textBlock)
        {
            writer.WriteStartElement("TEXT");
            writer.WriteAttributeString("x", textBlock.X.ToString());
            writer.WriteAttributeString("y", textBlock.Y.ToString());
            writer.WriteAttributeString("width", textBlock.Width.ToString());
            writer.WriteAttributeString("height", textBlock.Height.ToString());
            writer.WriteAttributeString("id", indexGenerator.TextIndex);

            foreach (var tokenBlock in textBlock.TokenBlocks.Where(x => !x.IsEmpty))
            {
                this.WriteTokenBlock(indexGenerator, writer, tokenBlock);
            }

            writer.WriteEndElement();
        }

        private void WriteTokenBlock(IndexGenerator indexGenerator, XmlWriter writer, TokenBlock tokenBlock)
        {
            if (tokenBlock.Text.All(Char.IsControl))
            {
                return;
            }

            writer.WriteStartElement("TOKEN");
            writer.WriteAttributeString("id", indexGenerator.TokenIndex);
            writer.WriteAttributeString("sid", indexGenerator.SidIndex);
            writer.WriteAttributeString("font-name", tokenBlock.FontName.FullName.ToLower());
            writer.WriteAttributeString("bold", tokenBlock.IsBold ? "yes" : "no");
            writer.WriteAttributeString("italic", tokenBlock.IsItalic ? "yes" : "no");
            writer.WriteAttributeString("font-size", tokenBlock.FontSize.ToString());
            writer.WriteAttributeString("font-color", tokenBlock.FontColor);
            writer.WriteAttributeString("rotation", "0"); // TODO(boumenot): support at some point ?!?
            writer.WriteAttributeString("angle", "0"); // TODO(boumenot): support at some point ?!?
            writer.WriteAttributeString("x", tokenBlock.X.ToString());
            writer.WriteAttributeString("y", tokenBlock.Y.ToString());
            writer.WriteAttributeString("base", tokenBlock.Base.ToString());
            writer.WriteAttributeString("width", tokenBlock.Width.ToString());
            writer.WriteAttributeString("height", tokenBlock.Height.ToString());

            writer.WriteString(tokenBlock.Text);
            writer.WriteEndElement();
        }
    }
}