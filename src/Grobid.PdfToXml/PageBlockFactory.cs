using System.Collections.Generic;
using System.IO;
using System.Linq;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Grobid.PdfToXml
{
    public class PageBlockFactory
    {
        private readonly TextBlockFactory textBlockFactory;

        public PageBlockFactory()
        {
            this.textBlockFactory = new TextBlockFactory();
        }

        public PageBlock[] Create(Stream stream)
        {
            return this.Create(stream, int.MaxValue);
        }

        public PageBlock[] Create(Stream stream, int maxPagesToRead)
        {
            using (var reader = new PdfReader(stream))
            {
                return this.ReadPages(reader).Take(maxPagesToRead).ToArray();
            }
        }

        private IEnumerable<PageBlock> ReadPages(PdfReader reader)
        {
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                var tokenBlocks = this.GetTokenBlocks(reader, i);
                var textBlocks = this.textBlockFactory.Create(tokenBlocks, reader.GetPageSize(i).Height);

                var pageBlock = this.CreatePageBlock(
                    reader.GetPageSize(i).Width,
                    reader.GetPageSize(i).Height,
                    i,
                    textBlocks);

                yield return pageBlock;
            }
        }

        private PageBlock CreatePageBlock(float width, float height, int offset, TextBlock[] textBlocks)
        {
            var pageBlock = new PageBlock
            {
                Width = width,
                Height = height,
                Offset = offset,
                TextBlocks = textBlocks,
            };

            return pageBlock;
        }

        private TokenBlock[] GetTokenBlocks(PdfReader reader, int pageNumber)
        {
            var tokenBlocks = new List<TokenBlock>();
            var pageSize = reader.GetPageSize(pageNumber);
            var tokenBlockFactory = new TokenFactory(pageSize.Width, pageSize.Height);

            var xmlTextExtractionStrategy = new XmlTextExtractionStrategy(tokenBlocks, tokenBlockFactory);

            PdfTextExtractor.GetTextFromPage(reader, pageNumber, xmlTextExtractionStrategy);
            return tokenBlocks.ToArray();
        }
    }
}
