using System.Collections.Generic;
using System.Linq;

namespace Grobid.PdfToXml
{
    public class TextBlockFactory
    {
        public TextBlock[] Create(TokenBlock[] tokenBlocks, float pageHeight)
        {
            return this.GetTextBlocks(tokenBlocks, pageHeight).ToArray();
        }

        private IEnumerable<TokenBlock> Merge(TokenBlock[] tokenBlocks)
        {
            int offset = 0;
            while (offset < tokenBlocks.Length)
            {
                offset += tokenBlocks
                    .Skip(offset)
                    .TakeWhile(x => x.IsEmpty)
                    .Count();

                var workingSet = tokenBlocks
                    .Skip(offset)
                    .TakeWhile(x => !x.IsEmpty)
                    .ToArray();

                offset += workingSet.Length;
                if (workingSet.Length == 0)
                {
                    break;
                }

                yield return TokenBlock.Merge(workingSet);
            }
        }

        private IEnumerable<TextBlock> GetTextBlocks(TokenBlock[] tokenBlocks, float pageHeight)
        {
            var i = 0;
            while (i < tokenBlocks.Length)
            {
                var y = tokenBlocks
                    .Skip(i)
                    .SkipWhile(x => x.IsEmpty)
                    .First()
                    .BoundingRectangle
                    .Bottom;

                var tokenBlocksOnSameLine = tokenBlocks
                    .Skip(i)
                    .TakeWhile(x => x.IsEmpty || x.BoundingRectangle.Bottom == y)
                    .ToArray();

                var mergedTokenBlocks = this.Merge(tokenBlocksOnSameLine)
                    .ToArray();

                yield return new TextBlock(mergedTokenBlocks, pageHeight);
                i += tokenBlocksOnSameLine.Length;
            }
        }
    }
}
