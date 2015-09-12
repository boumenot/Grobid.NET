using System.Collections.Generic;
using System.Linq;

namespace Grobid.NET
{
    public class TextBlockFactory
    {
        public IEnumerable<TextBlock> Create(PageBlock pageBlock)
        {
            var i = 0;
            while (i < pageBlock.TextInfos.Count)
            {
                var y = pageBlock.TextInfos[i].BoundingRectangle.Bottom;

                var textBlocks = pageBlock.TextInfos
                    .Skip(i)
                    .TakeWhile(x => x.IsEmpty || x.BoundingRectangle.Bottom == y);

                var xs = textBlocks.ToArray();
                yield return new TextBlock(xs, pageBlock.Height);

                i += xs.Length;
            }
        }
    }
}
