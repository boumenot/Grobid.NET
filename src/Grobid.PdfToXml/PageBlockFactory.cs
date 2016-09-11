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
            int tokenBlockCount = 0;
            int textBlockCount = 0;
            int blockCount = 0;

            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                var tokenBlocks = this.GetTokenBlocks(reader, i, tokenBlockCount);
                var textBlocks = this.textBlockFactory.Create(tokenBlocks, reader.GetPageSize(i).Height, textBlockCount);
                var blocks = this.PartitionIntoBlocks(textBlocks, i - 1, blockCount);

                var pageBlock = this.CreatePageBlock(
                    reader.GetPageSize(i).Width,
                    reader.GetPageSize(i).Height,
                    i,
                    blocks);

                yield return pageBlock;

                tokenBlockCount += tokenBlocks.Length;
                textBlockCount += textBlocks.Length;
                blockCount += blocks.Length;
            }
        }

        private Block[] PartitionIntoBlocks(TextBlock[] textBlocks, int page, int id)
        {
            var xss = new List<List<TextBlock>>
            {
                new List<TextBlock> { textBlocks[0] }
            };

            int index = 1;
            int offset = 0;

            while (index < textBlocks.Length)
            {
                for (; index < textBlocks.Length; index++)
                {
                    if (xss[offset].Last().IsSameBlock(textBlocks[index]))
                    {
                        xss[offset].Add(textBlocks[index]);
                    }
                    else
                    {
                        xss.Add(
                            new List<TextBlock>
                            {
                                textBlocks[index]
                            });
                        break;
                    }
                }

                offset++;
                index++;
            }

            return xss.Select((x,i) => new Block { Id = id + i, TextBlocks = x.ToArray(), Page = page }).ToArray();
        }

        private PageBlock CreatePageBlock(float width, float height, int offset, Block[] blocks)
        {
            var pageBlock = new PageBlock
            {
                Id = offset - 1, // zero-based counting
                Offset = offset, // one-based counting
                Height = height,
                Width = width,
                Blocks = blocks,
            };

            return pageBlock;
        }

        private TokenBlock[] GetTokenBlocks(PdfReader reader, int pageNumber, int id)
        {
            var tokenBlocks = new List<TokenBlock>();
            var pageSize = reader.GetPageSize(pageNumber);
            var tokenBlockFactory = new TokenBlockFactory(pageSize.Width, pageSize.Height, id);

            var xmlTextExtractionStrategy = new XmlTextExtractionStrategy(tokenBlocks, tokenBlockFactory);

            PdfTextExtractor.GetTextFromPage(reader, pageNumber, xmlTextExtractionStrategy);
            return tokenBlocks.ToArray();
        }
    }
}
