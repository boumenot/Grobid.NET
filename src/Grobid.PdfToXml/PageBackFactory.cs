using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Grobid.PdfToXml
{
    public class PageBackFactory
    {
        private readonly int maxPagesToRead;

        public PageBackFactory() : this(int.MaxValue) {}

        public PageBackFactory(int maxPagesToRead)
        {
            this.maxPagesToRead = maxPagesToRead;
        }

        public PageBlock[] Create(Stream stream)
        {
            using (var reader = new PdfReader(stream))
            {
                return this.ReadPages(reader).ToArray();
            }
        }

        private IEnumerable<PageBlock> ReadPages(PdfReader reader)
        {
            int pagesToRead = Math.Min(reader.NumberOfPages, this.maxPagesToRead);

            for (int i = 1; i <= pagesToRead; i++)
            {
                var tokenBlocks = this.GetTokenBlocks(reader, i);

                var pageBlock = this.CreatePageBlock(
                    reader.GetPageSize(i).Width,
                    reader.GetPageSize(i).Height,
                    tokenBlocks);

                yield return pageBlock;
            }
        }

        private PageBlock CreatePageBlock(float width, float height, List<TokenBlock> tokenBlocks)
        {
            var pageBlock = new PageBlock
            {
                Width = width,
                Height = height,
                TokenBlocks = tokenBlocks,
            };
            return pageBlock;
        }

        private List<TokenBlock> GetTokenBlocks(PdfReader reader, int pageNumber)
        {
            var tokenBlocks = new List<TokenBlock>();
            var pageSize = reader.GetPageSize(pageNumber);

            var xmlTextExtractionStrategy = new XmlTextExtractionStrategy(tokenBlocks, pageSize.Width, pageSize.Height);

            PdfTextExtractor.GetTextFromPage(reader, pageNumber, xmlTextExtractionStrategy);
            return tokenBlocks;
        }
    }
}
