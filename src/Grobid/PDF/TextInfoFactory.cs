using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Grobid.NET
{
    public class TextInfoFactory
    {
        private readonly int maxPagesToRead;

        public TextInfoFactory() : this(int.MaxValue) {}

        public TextInfoFactory(int maxPagesToRead)
        {
            this.maxPagesToRead = maxPagesToRead;
        }

        public IReadOnlyList<TextInfo>[] Create(Stream stream)
        {
            using (var reader = new PdfReader(stream))
            {
                return this.ReadPages(reader).ToArray();
            }
        }

        private IEnumerable<IReadOnlyList<TextInfo>> ReadPages(PdfReader reader)
        {
            int pagesToRead = Math.Min(reader.NumberOfPages, this.maxPagesToRead);

            for (int i = 1; i <= pagesToRead; i++)
            {
                var textInfos = new List<TextInfo>();
                var xmlTextExtractionStrategy = new XmlTextExtractionStrategy(textInfos);

                PdfTextExtractor.GetTextFromPage(reader, i, xmlTextExtractionStrategy);
                yield return textInfos;
            }
        }
    }
}
