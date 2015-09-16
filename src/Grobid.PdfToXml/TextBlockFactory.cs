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

        private IEnumerable<TextBlock> GetTextBlocks(TokenBlock[] tokenBlocks, float pageHeight)
        {
            var i = 0;
            while (i < tokenBlocks.Length)
            {
                var y = tokenBlocks[i].BoundingRectangle.Bottom;

                var textBlocks = tokenBlocks
                    .Skip(i)
                    .TakeWhile(x => x.IsEmpty || x.BoundingRectangle.Bottom == y);

                var xs = textBlocks.ToArray();
                yield return new TextBlock(xs, pageHeight);

                i += xs.Length;
            }
        }
    }
}
