using System.Collections.Generic;
using System.Linq;

namespace Grobid.PdfToXml
{
    public class TextBlockFactory
    {
        public TextBlock[] Create(PageBlock pageBlock)
        {
            return this.GetTextBlocks(pageBlock).ToArray();
        }

        private IEnumerable<TextBlock> GetTextBlocks(PageBlock pageBlock)
        {
            var i = 0;
            while (i < pageBlock.TokenBlocks.Count)
            {
                var y = pageBlock.TokenBlocks[i].BoundingRectangle.Bottom;

                var textBlocks = pageBlock.TokenBlocks
                    .Skip(i)
                    .TakeWhile(x => x.IsEmpty || x.BoundingRectangle.Bottom == y);

                var xs = textBlocks.ToArray();
                yield return new TextBlock(xs, pageBlock.Height);

                i += xs.Length;
            }
        }
    }
}
