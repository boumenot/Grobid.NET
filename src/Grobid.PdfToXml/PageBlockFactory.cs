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
                var blocks = this.PartitionIntoBlocks(textBlocks, i - 1);

                var pageBlock = this.CreatePageBlock(
                    reader.GetPageSize(i).Width,
                    reader.GetPageSize(i).Height,
                    i,
                    blocks);

                yield return pageBlock;
            }
        }

        private Block[] PartitionIntoBlocks(TextBlock[] textBlocks, int page)
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

            return xss.Select(x => new Block { TextBlocks = x.ToArray(), Page = page }).ToArray();
        }

        private PageBlock CreatePageBlock(float width, float height, int offset, Block[] blocks)
        {
            var pageBlock = new PageBlock
            {
                Width = width,
                Height = height,
                Offset = offset,
                Blocks = blocks,
            };

            return pageBlock;
        }

        private TokenBlock[] GetTokenBlocks(PdfReader reader, int pageNumber)
        {
            var tokenBlocks = new List<TokenBlock>();
            var pageSize = reader.GetPageSize(pageNumber);
            var tokenBlockFactory = new TokenBlockFactory(pageSize.Width, pageSize.Height);

            var xmlTextExtractionStrategy = new XmlTextExtractionStrategy(tokenBlocks, tokenBlockFactory);

            PdfTextExtractor.GetTextFromPage(reader, pageNumber, xmlTextExtractionStrategy);
            return tokenBlocks.ToArray();
        }
    }
}
